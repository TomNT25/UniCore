# Dev deploy: OCR + Face AI + UniCore.API (LAN / VM)

Local team setup (confirmed): **API + SQL Server on same VM**, **AI services on AI VM**, developers call API over `172.29.50.x`.

## Topology

```text
[Developer laptop]
    │  HTTP :5290 (Swagger, FE, Postman)
    ▼
┌──────────────── VM API (example 172.29.50.31) ────────────────┐
│  UniCore.API :5290                                            │
│  SQL Server     localhost / 127.0.0.1 (same VM only)          │
│  appsettings:   AiOcr / FaceAi → AI VM IP                     │
└───────────────────────────┬───────────────────────────────────┘
                            │ HTTP :8000 / :8001 (from API VM)
                            ▼
┌──────────────── VM AI (example 172.29.50.34) ─────────────────┐
│  OCR  VietOCR + PyTorch   :8000   POST /ocr  (field file)     │
│  Face ONNX Runtime        :8001   enroll / recognize          │
│  Embeddings stored on AI side (persist AI data directory)     │
└───────────────────────────────────────────────────────────────┘
```

| Component | GPU required? | Notes |
|-----------|---------------|--------|
| OCR | No (CUDA optional) | CPU fallback OK |
| Face | No (`CPUExecutionProvider`) | CUDA optional for speed |

## Configuration (on **API VM** only)

Copy `UniCore.Backend/UniCore.API/appsettings.Example.json` → `appsettings.json` (gitignored).

| Key | Example | Who reads it |
|-----|---------|--------------|
| `Database:ConnectionString` | `Data Source=localhost;Initial Catalog=UniCoreDatabase;...` | API on VM |
| `AiOcr:BaseUrl` | `http://172.29.50.34:8000` | API → OCR |
| `FaceAi:BaseUrl` | `http://172.29.50.34:8001` | API → Face |

Developers on other PCs **do not** put SQL connection strings on their laptop unless using SSMS against the VM.

CORS for FE is in `UniCore.API/Extensions/CorsExtension.cs` (team LAN origins).

## Start order (API VM)

1. SQL Server service running, database deployed + seed.
2. AI VM: start OCR (8000) and Face (8001).
3. API VM: `dotnet run` in `UniCore.Backend/UniCore.API` (port 5290).
4. From laptop: open `http://<API-VM-IP>:5290/swagger`.

## Contracts (BE ↔ AI)

### OCR (CCCD — **front side only**, 1 image)

| Layer | Detail |
|-------|--------|
| BE endpoint | `POST /api/v1/identity/cccd/ocr` (JWT), form field **`image`**, max **10 MiB** |
| BE → AI | `POST {AiOcr:BaseUrl}/ocr`, multipart field **`file`** |
| AI response | `{ success, data, error }` |

### Face

| Layer | Detail |
|-------|--------|
| Enroll | `POST /api/v1/auth/face/enroll` (JWT), `face_1` … `face_5` |
| Set PIN | `POST /api/v1/auth/face/set-pin` — **6 digits** + confirm, BCrypt in SQL |
| Login | `POST /api/v1/auth/face/login` (anonymous), field **`face`** → challenge token |
| Verify PIN | `POST /api/v1/auth/face/verify-pin` → JWT |
| BE → AI | `/ai/face/enroll`, `/ai/face/recognize` on Face VM |
| SQL | `user_face_profiles.embedding_id` only; vectors on **AI** |

FE is not in this repo — use field names above when integrating.

## Smoke tests (from repo root)

```bash
# Defaults: API localhost:5290, AI .34:8000/8001 — override env:
export API_BASE=http://172.29.50.31:5290
export AI_OCR_BASE=http://172.29.50.34:8000
export AI_FACE_BASE=http://172.29.50.34:8001

./scripts/smoke-ai.sh
# With sample images:
./scripts/smoke-ai.sh ./samples/cccd-front.jpg ./samples/face.jpg
```

Scripts:

| Script | Purpose |
|--------|---------|
| `scripts/smoke-ai.sh` | Runs direct AI checks + API checks |
| `scripts/smoke-ai-direct.sh` | FastAPI reachability + optional curl to OCR/Face |
| `scripts/smoke-ai-via-api.sh` | Login, face status, optional CCCD OCR via BE |

Manual REST: `UniCore.Backend/UniCore.API/Identity-Face.http`.

## Direct curl (debug from API VM or laptop if firewall allows)

```bash
# OCR (AI VM)
curl -s -X POST "$AI_OCR_BASE/ocr" -F "file=@cccd-front.jpg"

# Face enroll (AI VM — low-level; prefer BE enroll)
curl -s -X POST "$AI_FACE_BASE/ai/face/enroll" \
  -F "user_id=usr-student-001" -F "username=student1" -F "face_1=@face.jpg"
```

## Troubleshooting

| Symptom | Check |
|---------|--------|
| API 502/timeout on OCR/Face | From **API VM**, `curl http://172.29.50.34:8000/docs` |
| SQL login failed | Connection string must be **localhost** on API VM |
| CCCD 400 | Image &gt; 10 MiB or wrong field name (`image` on BE) |
| Face enroll OK but login fail | PIN not set (`FACE_PENDING_PIN`), or embedding lost on AI restart |
| CORS from FE | Add FE origin to `CorsExtension.cs` |

## Security (local LAN)

- Do not expose ports 8000/8001 to the public internet.
- Keep JWT secret and SQL password only in `appsettings.json` on the API VM.
