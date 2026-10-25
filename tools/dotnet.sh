#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
export DOTNET_ROOT="$(dirname "$PWD")/.ecosisteme-tools/dotnet"
export PATH="$DOTNET_ROOT:$PATH"
export DOTNET_CLI_HOME="$PWD/.local/dotnet"
export NUGET_PACKAGES="$PWD/.local/nuget/packages"
export XDG_DATA_HOME="$PWD/.local/share"
export XDG_CACHE_HOME="$PWD/.local/cache"
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
exec "$DOTNET_ROOT/dotnet" "$@"
