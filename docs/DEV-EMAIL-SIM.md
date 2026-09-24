# Dev: simulated email (StimulationEmailProvider)

**Giả lập email (simulation)** — không phải SIM điện thoại. Mail được lưu in-memory trên VM API tại `http://127.0.0.1:5289` để test announcement.

Announcement **IMPORTANT/URGENT** emails go through `IEmailSender`. On dev VM, use **HttpSimulation** instead of real SMTP.

## Topology (same VM as API)

```text
┌──────────── VM API ─────────────────────────────────────────────┐
│  StimulationEmailProvider   http://127.0.0.1:5289               │
│  UniCore.API                http://0.0.0.0:5290                 │
│  SQL Server                 localhost                           │
└─────────────────────────────────────────────────────────────────┘
```

Laptop teammates call **API** only (`172.29.50.31:5290`). Email sim listens on **127.0.0.1:5289** on that VM (not exposed to LAN unless you choose to).

## Start (on API VM)

Terminal 1 — inbox simulator:

```bash
cd UniCore.Backend/StimulationEmailProvider
dotnet run
```

Terminal 2 — API (with `appsettings.json`):

```bash
cd UniCore.Backend/UniCore.API
dotnet run
```

## appsettings.json (API)

```json
"Email": {
  "Enabled": true,
  "Provider": "HttpSimulation",
  "SimulationBaseUrl": "http://127.0.0.1:5289",
  "FromAddress": "noreply@unicore.local",
  "FromDisplayName": "UniCore Announcements"
}
```

Production later: `"Provider": "Smtp"` + Host/Port/credentials.

## Simulator API

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/email/send` | Accept `{ to, subject, body, fromAddress?, fromDisplayName? }` |
| GET | `/email/messages?page=&pageSize=` | Inbox list |
| GET | `/email/messages/{id}` | One message |
| DELETE | `/email/messages` | Clear inbox (dev) |

In-memory only — restart clears messages.

## Trigger announcement email (BE)

1. Seed **email whitelist** (`announcement_email_whitelists` / student `@unicore.edu.vn`).
2. Create announcement **type IMPORTANT or URGENT** with audience that resolves to whitelisted student emails.
3. Check inbox: `GET http://127.0.0.1:5289/email/messages`
4. Check DB: `announcement_email_logs`, admin **delivery-report**.

## Smoke test

```bash
./scripts/smoke-email.sh
```

## Troubleshooting

| Issue | Fix |
|-------|-----|
| `Cannot reach simulation email provider` | Start StimulationEmailProvider on 5289 |
| No emails in inbox | `Email:Enabled=false`, empty whitelist, or type NORMAL only |
| Logs Failed in DB | API cannot POST to 127.0.0.1:5289 |
