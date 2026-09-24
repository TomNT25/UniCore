#!/usr/bin/env bash
# Reachability + optional direct calls to OCR/Face FastAPI (bypass .NET).
# Env: AI_OCR_BASE (default http://172.29.50.34:8000), AI_FACE_BASE (default http://172.29.50.34:8001)
# Args: [cccd_front.jpg] [face.jpg]

set -euo pipefail

OCR_BASE="${AI_OCR_BASE:-http://172.29.50.34:8000}"
FACE_BASE="${AI_FACE_BASE:-http://172.29.50.34:8001}"
CCCD_FILE="${1:-}"
FACE_FILE="${2:-}"

red() { printf '\033[31m%s\033[0m\n' "$*"; }
green() { printf '\033[32m%s\033[0m\n' "$*"; }

http_code() { curl -s -o /dev/null -w "%{http_code}" "$@"; }

echo "=== Direct AI smoke (OCR + Face) ==="
echo "OCR:  $OCR_BASE"
echo "Face: $FACE_BASE"

for label in "OCR:$OCR_BASE/docs" "Face:$FACE_BASE/docs"; do
  name="${label%%:*}"
  url="${label#*:}"
  code="$(http_code "$url" || true)"
  if [[ "$code" == "200" ]]; then
    green "OK $name FastAPI docs ($code)"
  else
    red "FAIL $name docs unreachable (HTTP $code) — is uvicorn running?"
    exit 1
  fi
done

if [[ -n "$CCCD_FILE" && -f "$CCCD_FILE" ]]; then
  echo "POST $OCR_BASE/ocr ..."
  resp="$(curl -s -w "\n%{http_code}" -X POST "$OCR_BASE/ocr" -F "file=@${CCCD_FILE}")"
  code="$(echo "$resp" | tail -1)"
  body="$(echo "$resp" | sed '$d')"
  if [[ "$code" == "200" ]]; then
    green "OK OCR scan HTTP $code"
    echo "$body" | head -c 500
    echo ""
  else
    red "FAIL OCR HTTP $code"
    echo "$body"
    exit 1
  fi
else
  echo "(skip OCR POST — pass CCCD front image as arg 1)"
fi

if [[ -n "$FACE_FILE" && -f "$FACE_FILE" ]]; then
  echo "POST $FACE_BASE/ai/face/recognize ..."
  resp="$(curl -s -w "\n%{http_code}" -X POST "$FACE_BASE/ai/face/recognize" -F "face=@${FACE_FILE}")"
  code="$(echo "$resp" | tail -1)"
  body="$(echo "$resp" | sed '$d')"
  if [[ "$code" == "200" ]]; then
    green "OK Face recognize HTTP $code"
    echo "$body" | head -c 500
    echo ""
  else
    red "FAIL Face recognize HTTP $code"
    echo "$body"
    exit 1
  fi
else
  echo "(skip Face recognize — pass face image as arg 2)"
fi

green "Direct AI smoke finished."
