# Architecture — phase 00

`src/EcoSim.Contracts` → no project dependency; IDs, semantic schema and save gates, units.
`src/EcoSim.Core` → Contracts; immutable config/snapshots and deterministic step engine.
`src/EcoSim.Gameplay` → Core + Contracts; pause/input orchestration.
`unity-client/Assets/Scripts` → shared libraries + Unity; touch/mouse, diagnostic terrain/camera/probe, RO/EN controls.
`unity-client/Assets/Editor` → Editor build configuration + URP asset generation.
`tests` → .NET unit tests, fixture and Python architecture/build-guard tests.
`tools` → bootstrap, verification, toolchain pin and headless build wrapper.
`content` → future licensed art and sourced biological data, currently empty except policy.

The three source assemblies each expose a local UPM package, not generated DLL copies. Both compilers consume the same .cs files. The csproj target netstandard2.1 and noEngineReferences asmdefs prevent coupling to the presentation layer; reflection and source checks verify the boundary in .NET. Unity compilation is still NOT RUN.

Input/output contracts are in CONTRACTS.md. The scene reads snapshots; position, tick and diagnostic sample come from Core. Removing the renderer cannot change a simulation tick. All real ecological dynamics, profiles, saves and campaign systems belong to future phases. No established APIs or saves existed before this phase.
