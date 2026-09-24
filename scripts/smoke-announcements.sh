#!/usr/bin/env bash
# Smoke-test announcement APIs. Requires: curl, API up, valid seed + appsettings.
# Usage: ./scripts/smoke-announcements.sh [base_url] [admin_password] [student_password]

set -euo pipefail

BASE="${1:-http://localhost:5290}"
API="$BASE/api/v1"
ADMIN_PASS="${2:-Password123!}"
STUDENT_PASS="${3:-Password123!}"

red() { printf '\033[31m%s\033[0m\n' "$*"; }
green() { printf '\033[32m%s\033[0m\n' "$*"; }

extract_token() {
  local body="$1"
  if command -v jq >/dev/null 2>&1; then
    echo "$body" | jq -r '.data.accessToken // .data.access_token // empty'
  else
    echo "$body" | sed -n 's/.*"accessToken"[[:space:]]*:[[:space:]]*"\([^"]*\)".*/\1/p' | head -1
  fi
}

http_code() {
  curl -s -o /dev/null -w "%{http_code}" "$@"
}

login() {
  local user="$1" pass="$2"
  curl -s -X POST "$API/auth/login" \
    -H "Content-Type: application/json" \
    -d "{\"username\":\"$user\",\"password\":\"$pass\"}"
}

echo "=== UniCore announcement smoke test ==="
echo "Base: $API"

if [[ "$(http_code "$BASE/swagger/index.html")" != "200" ]]; then
  red "API not reachable at $BASE (start UniCore.API first)."
  exit 1
fi

ADMIN_BODY="$(login admin "$ADMIN_PASS")"
ADMIN_TOKEN="$(extract_token "$ADMIN_BODY")"
if [[ -z "$ADMIN_TOKEN" ]]; then
  red "Admin login failed. Check password (seed) and DB."
  echo "$ADMIN_BODY"
  exit 1
fi
green "Admin login OK"

STUDENT_BODY="$(login student1 "$STUDENT_PASS")"
STUDENT_TOKEN="$(extract_token "$STUDENT_BODY")"
if [[ -z "$STUDENT_TOKEN" ]]; then
  red "Student login failed."
  echo "$STUDENT_BODY"
  exit 1
fi
green "Student login OK"

auth_admin=(-H "Authorization: Bearer $ADMIN_TOKEN")
auth_student=(-H "Authorization: Bearer $STUDENT_TOKEN")

check() {
  local name="$1" expected="$2"
  shift 2
  local code
  code="$(http_code "$@")"
  if [[ "$code" == "$expected" ]]; then
    green "OK $name ($code)"
  else
    red "FAIL $name expected $expected got $code"
    exit 1
  fi
}

check "public list" 200 "$API/public/announcements"
check "admin list" 200 "${auth_admin[@]}" "$API/admin/announcements?pageNumber=1&pageSize=5"
check "admin targets departments" 200 "${auth_admin[@]}" "$API/admin/announcements/targets/departments?search=&limit=5"
check "student me active" 200 "${auth_student[@]}" "$API/student/announcements/me?status=active"
check "student get-new" 200 "${auth_student[@]}" "$API/student/announcements/get-new"

green "All smoke checks passed."
