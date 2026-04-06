#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ASSETS_FILE="$SCRIPT_DIR/../assets.json"

if [[ ! -f "$ASSETS_FILE" ]]; then
    echo "ERROR: assets.json not found at '$ASSETS_FILE'" >&2
    exit 1
fi

# Helper function to download one asset
_download_asset() {
    local url="$1"
    local dest="$2"

    local destdir
    destdir="$(dirname "$dest")"
    mkdir -p "$destdir"

    echo "Downloading: $url -> $dest"
    curl -fsSL "$url" -o "$dest"
}

# Download CSS assets
jq -r '.css[] | "\(.url)\t\(.dest)"' "$ASSETS_FILE" \
    | while IFS=$'\t' read -r url dest; do
        _download_asset "$url" "$dest"
      done

# Download JS assets
jq -r '.js[] | "\(.url)\t\(.dest)"' "$ASSETS_FILE" \
    | while IFS=$'\t' read -r url dest; do
        _download_asset "$url" "$dest"
      done