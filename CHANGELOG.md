# Change log

## Unreleased — phase 00, partial

- Added C# 9/netstandard2.1 Contracts, Core and Gameplay libraries with shared local UPM sources.
- Added deterministic clock, explicit seeded diagnostic samples, bounded input probe and pause session.
- Added schema/save version gates, stable IDs and SI unit contract; no existing saves to migrate.
- Added .NET/architecture tests, exact SDK pin, package locks, bootstrap and CI examples.
- Added pinned Unity/URP manifest, Android build guard/configuration and diagnostic touch scene source.
- Unity package resolution, Editor compilation, rendering, APK and physical-device validation remain unrun.

- Corrected Editor pin to6000.0.66f2 following Unity's official fix for falsely invalid package signatures; verification remains enabled.
