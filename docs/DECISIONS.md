# Decizii tehnice

## Pregătire cloud, 2026-10-25 — provizoriu

SDK .NET 10.0.402 LTS instalat separat în /workspace/.ecosisteme-tools/dotnet pentru verificarea infrastructurii C#. Nu stabilește framework-ul EcoSim.Core sau compatibilitatea Unity. Arhiva oficială Microsoft este verificată SHA-512 înainte de extragere. Unity, URP, SDK/NDK Android și targetSdk nu au fost selectate sau validate. Nicio cerință de produs modificată.

## ADR-0001 — Assembly boundaries (phase 00)

Accepted implementation choice: shared sources in three local UPM packages plus .NET Standard 2.1 projects, C# 9, no binary copies. SDK 10.0.402 hosts the tests/CLI only; it does not determine Unity's runtime. Generated .NET output lives under root .local rather than src package roots, preventing Unity from importing obj assembly metadata. Test framework xUnit 2.9.3 with locked dependencies. No previous API or save data exists.

## ADR-0002 — Foundation-only deterministic state

Clock, seeded xorshift32 diagnostic sample, reserved probe ID 1 and synchronous bounded placement are sufficient to exercise presentation/core separation without inventing biological constants. Diagnostic visuals are explicitly labeled. Save format 1/schema0.1.0 are gates, not an implemented persistence system. No ecological dynamics are claimed.

## ADR-0003 — Unity/Android deferred validation

Pin Unity6000.0.62f1 (official source tag verified) and URP17.0.4 (official source metadata declares6000.0 compatibility). Real UPM lock/Editor import remains blocked; do not invent a transitive lock. Generate URP assets through Editor APIs. Use IL2CPP/ARM64, landscape-left and minSdk34. Require explicit developer-reviewed target API and actual installed platform; policy compliance is not inferred from today's date. No paid license or eligibility is assumed. API36 exists in this pinned Editor's public AndroidSdkVersions enum; this does not establish current Play requirements or SDK installation.

## ADR-0004 — Build and licensing boundaries

Headless build is opt-in on an existing licensed Editor. No automatic Unity installation/activation, paid runner, publication or account creation. Default CI exercises .NET; optional self-hosted Android job runs only by manual request. Development APK uses debug signing; release signing and Play delivery are later work. Current sandbox's runtime mutex requires supported escalation; NuGet/TLS/signature checks remain enabled.

## ADR-0005 — Correct Editor signature validation (supersedes ADR-0003 Editor pin)

Pin Unity6000.0.66f2 instead of6000.0.62f1. The user supplied a screenshot of Unity staff's official post https://discussions.unity.com/t/package-manager-error-resolved-new-hub-badge-warning/1706569 stating affected6000.0 releases must update to6000.0.66f2 or later. Hub also displayed the signature-validation warning on6000.0.62f1. Verify signatures normally; never disable verification. New official UnityCsReference tag verified by git ls-remote:6472489f23c9ac92ba222a431597782907f5921f. Existing URP17.0.4 pin remains; actual import/package resolution/build still NOT RUN. Updated Editor version, build guard, CI runner label, toolchain metadata and current setup docs. Historical command/results in phase00 report are preserved.
