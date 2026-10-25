#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
sdk_dir="$(dirname "$PWD")/.ecosisteme-tools/dotnet"
mkdir -p "$sdk_dir"
if ! test -x "$sdk_dir/dotnet" || ! "$sdk_dir/dotnet" --list-sdks | grep -q '^10.0.402 '; then
  archive=$(mktemp /tmp/ecosisteme-sdk.XXXXXX.tar.gz)
  trap 'rm -f "$archive"' EXIT
  curl --fail --show-error --location --retry 2 https://builds.dotnet.microsoft.com/dotnet/Sdk/10.0.402/dotnet-sdk-10.0.402-linux-x64.tar.gz -o "$archive"
  printf '%s  %s\n' 1904a98dda2db34ba78ba3c4fb1b224455329af91c341d83547843c1839e112b04ae699ee92fa2449a8f62249196a938723078c0c60b9f65c88539d5687729c2 "$archive" | sha512sum --check -
  tar -xzf "$archive" -C "$sdk_dir"
fi
"$sdk_dir/dotnet" --version
