#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
export PYTHONDONTWRITEBYTECODE=1
python3 tools/validate-layout.py .
python3 -m unittest discover -s tests -p 'test_*.py' -v
bash tools/dotnet.sh restore EcoSim.sln --locked-mode
bash tools/dotnet.sh test EcoSim.sln -c Release --no-restore --logger 'trx;LogFileName=phase00.trx' --results-directory TestResults
bash tools/dotnet.sh run --project samples/EcoSim.Smoke/EcoSim.Smoke.csproj -c Release --no-restore
