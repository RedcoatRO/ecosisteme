# PHASE 00 — MASTER FOUNDATION

**Editor pin correction:** current version is6000.0.66f2, per official signature-validation fix supplied in the user screenshot. Original6000.0.62f1 references below record initial commands/evidence, not the current installation instruction. See ADR-0005. Unity/Android validation remains NOT RUN. UPM access was successfully retried after configuration publication.

**Status: PARTIAL / NOT ACCEPTED.** 5 checkpoints complete within their stated scope, 3 partial. Phase01 not started. Date: 2026-10-25 (Bucharest). This repository began empty, with valid symbolic unborn branch `work`, no HEAD commit, no source files and no pre-existing saves/API. Read-only `git ls-remote origin HEAD` succeeded with empty output; origin points to RedcoatRO/ecosisteme. No commit/push/publication was performed.

## Checkpoint matrix (8/8 reported)

| Checkpoint | Status | Actual implementation / inputs and outputs | Positive and negative/boundary evidence |
|---|---|---|---|
| 00.01 Architecture | COMPLETE | Required directories, shared assembly definitions, csproj references, editorconfig, gitignore; root input → validated boundaries | Python layout PASS; removing Core in a temporary fixture and inserting UnityEngine import are rejected; generated outputs excluded from Unity package roots |
| 00.02 Unity Android configuration | PARTIAL | Editor6000.0.62f1, pinned URP17.0.4 manifest, toolchain-lock metadata; Editor build preparation source for ARM64/IL2CPP, landscape-left, minSdk34, explicit targetSdk | Official Editor source tag and URP compatibility metadata verified; unsupported/absent target guarded. Real package restore, transitive UPM lock, generated ProjectSettings, Editor compile/open and actual installed targetSdk validation NOT RUN |
| 00.03 Engine-independent C# | COMPLETE | Three compilable netstandard2.1 libraries; WorldConfig → deterministic SimulationStep snapshots; CLI sample in samples/EcoSim.Smoke | Release compilation PASS; CLI produces tick4/time2s/probe1 at(2,-3); invalid seed/time/extent/null config/input rejected; no Unity assembly references |
| 00.04 Versioned contracts | COMPLETE | Stable IDs, schema0.1.0, save format1 and SI units; compatibility policy in CONTRACTS.md | Max ulong ID round-trip PASS; zero/overflow/invalid ID and incompatible future/old save versions rejected. No serialization or migration claimed; no prior saves |
| 00.05 Test harness | COMPLETE for available tooling | xUnit suite and NuGet lockfiles, deterministic fixture, Python boundary/guard/document tests | 39 .NET cases PASS, no failed/skipped cases; 8 Python cases PASS,0 failed/skipped. Same-seed ordered-command replay1000 steps; known PRNG vector. Unity edit-mode tests NOT RUN: no Editor/tooling, conditional requirement not applicable here |
| 00.06 Build pipelines | PARTIAL | Fail-fast headless script and Editor build entrypoint; core CI and manual licensed self-hosted Android CI example | Local core pipeline equivalent PASS; missing Editor and invalid target return exit2 before build. No licensed Editor success branch, remote CI execution, IL2CPP build, APK or signing/release validation |
| 00.07 Operating guidelines | COMPLETE | AGENTS, decision/change logs, contributions, commands, troubleshooting, license inventory | Documented bootstrap repeated successfully; verification commands executed. README links checked; negative broken-document fixture and actionable build guards verified |
| 00.08 Android smoke scene | PARTIAL | Source scene + stable meta IDs; flat primitive terrain, camera/light, touch/mouse placement, pause, RO/EN; view reads Core snapshots | Core placement/boundary/pause tests PASS; diagnostic size derives from simulation sample. Scene import/rendering/touch/glyphs/landscape/offline/device acceptance NOT RUN. No screenshot, APK or performance result |

## Evidence and commands

Working directory `/workspace/ecosisteme` unless otherwise stated. Final results are in `reports/evidence/phase00-verify.log`; retained TRX is `reports/evidence/phase00-results.trx`, copied from the current run in `TestResults/phase00.trx` (ignored working output). Final count comes from the final log; previous intermediate runs had fewer Python tests. No zero-test run is counted as validation.

Exact principal commands actually executed:

```bash
# Initial inspection
git -C /workspace/ecosisteme status --short
git -C /workspace/ecosisteme ls-files
git -C /workspace/ecosisteme rev-parse --verify HEAD
git -C /workspace/ecosisteme symbolic-ref HEAD
git -C /workspace/ecosisteme ls-remote origin HEAD
java -version
uname -m
df -h /workspace
# Tool/source evidence (network-policy read preceded network checks)
curl -fsSL https://builds.dotnet.microsoft.com/dotnet/release-metadata/10.0/releases.json -o /tmp/ecosisteme-dotnet10.json
git ls-remote --tags https://github.com/Unity-Technologies/UnityCsReference.git 'refs/tags/6000.0.62f1'
git ls-remote https://github.com/Unity-Technologies/Graphics.git refs/heads/6000.0/staging refs/tags/6000.0.62f1
curl -fsSL https://packages.unity.com/com.unity.render-pipelines.universal -o /tmp/ecosisteme-upm.json
curl -fsSL 'https://services.api.unity.com/unity/editor/release/v1/releases?version=6000.0.62f1&limit=1' -o /tmp/ecosisteme-unity.json
# Setup / validation
bash tools/cloud-install.sh
bash tools/dotnet.sh build src/EcoSim.Gameplay/EcoSim.Gameplay.csproj -c Release
bash tools/dotnet.sh new sln --name EcoSim --format sln
bash tools/dotnet.sh sln EcoSim.sln add src/EcoSim.Contracts/EcoSim.Contracts.csproj src/EcoSim.Core/EcoSim.Core.csproj src/EcoSim.Gameplay/EcoSim.Gameplay.csproj tests/EcoSim.Tests/EcoSim.Tests.csproj
bash tools/dotnet.sh test EcoSim.sln -c Release --logger 'trx;LogFileName=phase00.trx' --results-directory TestResults
bash tools/dotnet.sh sln EcoSim.sln add samples/EcoSim.Smoke/EcoSim.Smoke.csproj
bash tools/dotnet.sh restore EcoSim.sln
bash -n tools/cloud-install.sh tools/dotnet.sh tools/verify.sh tools/unity-build.sh
bash tools/unity-build.sh
set -o pipefail
bash tools/verify.sh 2>&1 | tee reports/evidence/phase00-verify.log
git diff --check
```

The install script contains the exact SDK URL/hash and executes SHA512 verification before extraction; archive check PASS. Repeated bootstrap returned10.0.402 without re-downloading. Native sandbox .NET initially failed before compilation with `NuGet-Migrations` IOException; strace isolated `mkdir("/home/agent/.dotnet") = EROFS`. DOTNET_CLI_HOME/TMPDIR relocation and a specific permission grant did not fix the read-only mount. Supported `require_escalated` execution succeeded. No TLS/signature/checksum verification was disabled. Raw trace/SDK/cache are local ignored data, not product sources.

Successful source queries confirmed Unity source tag commit `d1fc730d54e0ef53c442c2e740d7aabf62fbdae8` and Graphics commit `feb4de2d9a93a4ae10d287d3a6d7003d08ea3e53`. Official raw package/API sources were inspected at these versions. UPM and release API curl commands returned HTTP403 (exit22), so no package lock/download success is claimed. Java21/x64 and approximately30GiB free workspace were observed; Java21 is not certified for Unity Android. `unity-build.sh` returned expected actionable exit2 for missing Editor. It did not execute Unity or produce an APK.

## Files created and retained

All project files are new, uncommitted local files in the requested repository. No pre-existing tracked files were altered. Root: .editorconfig, .gitignore, global.json, Directory.Build.props, EcoSim.sln, README, AGENTS, CONTRIBUTING, CHANGELOG. Code: src/EcoSim.Contracts (IDs/schema/units), src/EcoSim.Core (config/step), src/EcoSim.Gameplay (session), their csproj/asmdef/package metadata and NuGet locks. Sample: samples/EcoSim.Smoke and lock. Tests: tests/EcoSim.Tests and lock, fixtures, Python tests. Unity: ProjectVersion.txt, Packages/manifest.json, Assets/Editor/CloudBuild.cs, Assets/Scripts/SmokeWorld.cs, Assets/Scenes/Smoke.unity, asmdefs and .meta files. Tooling: tools/*.sh, validate-layout.py, toolchain-lock.json, .github/workflows/foundation.yml. Documentation: docs/*.md, content/README.md, this report, BLOCKERS.md and verification evidence.

Ignored generated outputs: .local build/cache folders, TestResults, eventual Unity Library/Generated and builds. SDK retained outside checkout at /workspace/.ecosisteme-tools/dotnet. Initial bin/obj outputs generated by our commands were moved to .local/previous-build before output-path correction; no user data was removed. Nothing external was published.

## Acceptance checklist

- [x] No runtime dependency from EcoSim.Core to UnityEngine: source check, asmdef boundary and reflected references PASS.
- [x] Core unit tests pass:39 cases,0 failed,0 skipped; meaningful fixture/determinism/boundary/compatibility coverage.
- [x] Exact Unity blocker report supplied as permitted by criterion; **actual project opening/compilation remains NOT RUN**.
- [ ] Full reproducible Unity resolution: real packages-lock.json and Editor-generated settings missing.
- [ ] Current Play target policy + installed SDK/JDK/NDK validation: NOT RUN.
- [ ] Actual Android APK path/launch evidence: NOT RUN; candidate output path is only documented.
- [ ] Xiaomi12TPro scene/touch/offline/performance acceptance: NOT RUN.

## Risks, defects and next action

No failing core test or diagnosed application defect remains. Untested Unity/URP/IL2CPP integration can still contain compile/import/rendering problems; static source inspection is not proof. The legacy input setting is configured through a checked serialized Editor property and must be confirmed in the pinned Editor. The diagnostic scene uses IMGUI and primitive terrain; adaptive detail, ecology, real saves and campaigns are not phase00 implementations. World clock values and probe extents are software sample assumptions, not biological measurements. Default StableId is invalid; future serializers must enforce nonzero IDs.

Manual remediation and exact local commands are in docs/DEVELOPMENT.md; external prerequisites are in reports/BLOCKERS.md. Next safe action is real pinned Editor import/UPM resolution and Android toolchain validation, then scene/build/device checks. Preserve this foundation and do not proceed to phase01 until phase00 is accepted.

## Environment configuration

Saved draft confirmed: install_script, start_skill and additive custom domain list (previous custom list was empty): packages.unity.com, services.api.unity.com, download.unity3d.com, docs.unity3d.com, unity.com, support.google.com. Package-manager presets retained. No secrets added. Draft save does not execute scripts, apply network settings or publish a snapshot. Review and save in environment settings, then publish when ready. Current .NET development is validated with supported escalation; the full Unity/Android environment is not ready, and restoration in a new task is unverified.
