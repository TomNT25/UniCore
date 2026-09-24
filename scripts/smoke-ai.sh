#!/usr/bin/env bash
# Full AI smoke: direct FastAPI + UniCore API integration.
# See docs/DEV-AI-DEPLOY.md
# Usage: ./scripts/smoke-ai.sh [cccd_front.jpg] [face.jpg]

set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
CCCD="${1:-}"
FACE="${2:-}"

export API_BASE="${API_BASE:-http://172.29.50.31:5290}"
export AI_OCR_BASE="${AI_OCR_BASE:-http://172.29.50.34:8000}"
export AI_FACE_BASE="${AI_FACE_BASE:-http://172.29.50.34:8001}"

echo "API_BASE=$API_BASE"
echo "AI_OCR_BASE=$AI_OCR_BASE"
echo "AI_FACE_BASE=$AI_FACE_BASE"
echo ""

"$ROOT/scripts/smoke-ai-direct.sh" "$CCCD" "$FACE"
echo ""
"$ROOT/scripts/smoke-ai-via-api.sh" "${SMOKE_PASSWORD:-Password123!}" "$CCCD" "$FACE"
