#!/usr/bin/env bash
# Smoke OCR/Face integration through UniCore.API.
# Env: API_BASE (default http://172.29.50.31:5290)
# Usage: ./smoke-ai-via-api.sh [password] [cccd_front.jpg] [face.jpg]

set -euo pipefail

BASE="${API_BASE:-http://172.29.50.31:5290}"
API="$BASE/api/v1"
PASS="${1:-Password123!}"
CCCD_FILE="${2:-}"
FACE_FILE="${3:-}"

red() { printf '\033[31m%s\033[0m\n' "$*"; }
green() { printf '\033[32m%s\033[0m\n' "$*"; }

extract_token() {
  local body="$1"
  if command -v jq >/dev/null 2>&1; then
    echo "$body" | jq -r '.data.accessToken // empty'
  else
    echo "$body" | sed -n 's/.*"accessToken"[[:space:]]*:[[:space:]]*"\([^"]*\)".*/\1/p' | head -1
  fi
}

http_code() { curl -s -o /dev/null -w "%{http_code}" "$@"; }

echo "=== UniCore API smoke (Identity + Face) ==="
echo "API: $API"

if [[ "$(http_code "$BASE/swagger/index.html")" != "200" ]]; then
  red "API not reachable at $BASE"
  exit 1
fi
green "OK Swagger reachable"

login_body="$(curl -s -X POST "$API/auth/login" \
  -H "Content-Type: application/json" \
  -d "{\"username\":\"student1\",\"password\":\"$PASS\"}")"
TOKEN="$(extract_token "$login_body")"
if [[ -z "$TOKEN" ]]; then
  red "student1 login failed"
  echo "$login_body"
  exit 1
fi
green "OK student1 login"

auth=(-H "Authorization: Bearer $TOKEN")

code="$(http_code "${auth[@]}" "$API/auth/face/status")"
if [[ "$code" == "200" ]]; then
  green "OK GET /auth/face/status ($code)"
else
  red "FAIL GET /auth/face/status ($code)"
  exit 1
fi

code="$(http_code "${auth[@]}" "$API/identity/cccd")"
if [[ "$code" == "200" ]]; then
  green "OK GET /identity/cccd ($code)"
else
  red "FAIL GET /identity/cccd ($code)"
  exit 1
fi

if [[ -n "$CCCD_FILE" && -f "$CCCD_FILE" ]]; then
  resp="$(curl -s -w "\n%{http_code}" "${auth[@]}" -X POST "$API/identity/cccd/ocr" -F "image=@${CCCD_FILE}")"
  code="$(echo "$resp" | tail -1)"
  body="$(echo "$resp" | sed '$d')"
  if [[ "$code" == "200" ]]; then
    green "OK POST /identity/cccd/ocr ($code)"
    echo "$body" | head -c 400
    echo ""
  else
    red "FAIL POST /identity/cccd/ocr ($code)"
    echo "$body"
    exit 1
  fi
else
  echo "(skip CCCD OCR — pass front image as arg 2)"
fi

if [[ -n "$FACE_FILE" && -f "$FACE_FILE" ]]; then
  resp="$(curl -s -w "\n%{http_code}" -X POST "$API/auth/face/login" -F "face=@${FACE_FILE}")"
  code="$(echo "$resp" | tail -1)"
  body="$(echo "$resp" | sed '$d')"
  if [[ "$code" == "200" ]]; then
    green "OK POST /auth/face/login ($code) — check challenge in response"
    echo "$body" | head -c 400
    echo ""
  else
    echo "Face login returned HTTP $code (may be expected if not enrolled)"
    echo "$body" | head -c 400
    echo ""
  fi
else
  echo "(skip face login — pass face image as arg 3)"
fi

green "API smoke finished."
