#!/usr/bin/env bash
# Smoke test StimulationEmailProvider (default http://127.0.0.1:5289 on API VM).

set -euo pipefail

SIM_BASE="${EMAIL_SIM_BASE:-http://127.0.0.1:5289}"

red() { printf '\033[31m%s\033[0m\n' "$*"; }
green() { printf '\033[32m%s\033[0m\n' "$*"; }

http_code() { curl -s -o /dev/null -w "%{http_code}" "$@"; }

echo "=== Email simulation smoke ==="
echo "Base: $SIM_BASE"

if [[ "$(http_code "$SIM_BASE/health")" != "200" ]]; then
  red "Simulator not running. On API VM: cd UniCore.Backend/StimulationEmailProvider && dotnet run"
  exit 1
fi
green "OK GET /health"

if [[ "$(http_code "$SIM_BASE/email/messages?page=1&pageSize=1")" != "200" ]]; then
  red "Inbox not reachable at $SIM_BASE"
  exit 1
fi
green "OK GET /email/messages"

resp="$(curl -s -w "\n%{http_code}" -X POST "$SIM_BASE/email/send" \
  -H "Content-Type: application/json" \
  -d '{"to":"student1@unicore.edu.vn","subject":"Smoke test","body":"<p>Hello from smoke-email.sh</p>"}')"
code="$(echo "$resp" | tail -1)"
if [[ "$code" != "200" ]]; then
  red "FAIL POST /email/send ($code)"
  exit 1
fi
green "OK POST /email/send"

count="$(curl -s "$SIM_BASE/email/messages?page=1&pageSize=5" | grep -o '"total":[0-9]*' | head -1 || true)"
green "Inbox $count"
green "Email simulation smoke passed."
