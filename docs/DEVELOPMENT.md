# Development

## Bootstrap and required checks

From the existing repository root (cloud: /workspace/ecosisteme):

```bash
bash tools/cloud-install.sh
bash tools/verify.sh
```

Bootstrap is Linux x64, needs curl, tar and sha512sum, and installs SDK 10.0.402 in the checkout parent's `.ecosisteme-tools/dotnet`. It verifies Microsoft's published SHA-512 before extracting. The install is repeatable; scripts run in their own repository root. dotnet.sh sets DOTNET_ROOT/PATH plus local NuGet/CLI/cache directories and disables SDK telemetry. No HOME override or credentials are stored. A managed sandbox may need supported escalation because runtime mutexes require /home/agent/.dotnet even when CLI caches are relocated. Do not fabricate first-use sentinels or turn verification off. NuGet dependencies use committed lockfiles and locked-mode restore.

verify.sh checks architecture, Python tests, locked restore and Release .NET tests. TRX: TestResults/phase00.trx. A standalone diagnostic sample runs as part of verification: `bash tools/dotnet.sh run --project samples/EcoSim.Smoke/EcoSim.Smoke.csproj -c Release --no-restore`. This verifies libraries and harness, not Editor/rendering/Android. CI core uses the same scripts on Linux; its remote execution has not been observed. The optional Android job needs an existing licensed self-hosted runner and is manual only; no paid runner is provisioned.

## Unity / Android (NOT RUN here)

Install exactly Unity 6000.0.66f2 through official Unity tooling with Android Build Support, Android SDK/NDK and OpenJDK modules. Verify Personal eligibility and activate locally under the applicable terms. Keep activation data out of Git/chat. Read current Google Play target API policy at https://support.google.com/googleplay/android-developer/answer/11926878 . Confirm the Editor supports that API and the installed platform has android.jar. Use the bundled Unity JDK/NDK; the cloud's system Java is not validated for this build.

Set these variables to real installed paths and reviewed values (shell placeholders must be replaced):

```bash
export UNITY_EDITOR='/absolute/path/to/6000.0.66f2/Editor/Unity'
export ANDROID_SDK_ROOT='/absolute/path/to/6000.0.66f2/Editor/Data/PlaybackEngines/AndroidPlayer/SDK'
export ANDROID_TARGET_SDK='<numeric API from current Play policy and installed SDK>'
export PLAY_POLICY_CHECKED_ON='<UTC date yyyy-MM-dd when you actually reviewed policy>'
bash tools/unity-build.sh
```

The build entrypoint validates Editor version, Android module, explicit target API enum and installed android.jar. Unity validates remaining JDK/NDK/build-tools compatibility during build. Failure exits nonzero. It generates URP renderer/pipeline/material under ignored Assets/Generated, sets Android IL2CPP, ARM64, landscape-left, minSdk34, explicit targetSdk and GLES3, adds Smoke.unity, then builds a **development/debug-signed APK**. It is not a Play publication artifact. Real success would create builds/android/ecosisteme-smoke.apk and ECOSIM_APK_BUILT in builds/logs/unity-android.log. No such artifact is present now. Do not infer success from path text or a process.

First import must resolve UPM packages and generate Packages/packages-lock.json. Review and commit the real generated lock and ProjectSettings with the pinned Editor in a follow-up coding step. They cannot be certified or hand-generated here. For interactive use launch the pinned Editor with the same variables, open unity-client, choose EcoSim/Prepare Android smoke, open Assets/Scenes/Smoke.unity and press Play. Expected: flat green 20 m terrain, camera, yellow diagnostic probe; time increases each 0.5 s; tap ground moves ID 1; pause freezes ticks; RO/EN toggles text. Outside-world taps are ignored. Visual size is derived from the deterministic diagnostic sample. This is a smoke demonstration, not ecology.

## Device acceptance (all NOT RUN)

Connect the Xiaomi 12T Pro with USB debugging, verify identity and Android 14+ before installation:

```bash
adb devices -l
adb -s "$ANDROID_SERIAL" shell getprop ro.product.model
adb -s "$ANDROID_SERIAL" shell getprop ro.build.version.sdk
adb -s "$ANDROID_SERIAL" install -r builds/android/ecosisteme-smoke.apk
adb -s "$ANDROID_SERIAL" shell monkey -p ro.redcoat.ecosisteme -c android.intent.category.LAUNCHER 1
adb -s "$ANDROID_SERIAL" logcat -d -s Unity
```

Set ANDROID_SERIAL only after selecting the connected reference phone. Do not install if API <34. Capture launch log with ECOSIM_SMOKE_STARTED, screenshot/video of landscape terrain, touch placement/pause/RO/EN behavior, invalid outside-boundary tap behavior, app restart and offline airplane-mode launch. Record screenshot source and actual device/OS/build. A successful adb install alone does not validate gameplay. Measure frame pacing/thermal behavior before any sustained 30 FPS claim; phase00 does not implement adaptive detail tiers. RO/EN glyph/font rendering also needs device review.

## Troubleshooting

- HTTP 403 for UPM/Editor downloads: review environment network settings; required hosts are listed in TOOLCHAIN.md and draft. Do not bypass proxy or TLS.
- NuGet-Migrations IOException / read-only .dotnet: use supported sandbox escalation for the runtime state directory. Escalation worked in this session; plain sandbox execution remained blocked.
- Missing/generated URP material: run preparation command; inspect UPM errors, not an empty build log.
- Android API policy/SDK guard: install the reviewed platform through Unity's supported SDK tools; do not set an arbitrary API just to pass.
- Wrong Editor or missing Android module: install the exact version/modules. No unverified editor substitution.
- Negative .NET tests assert exceptions; they are passed tests, not expected failures/skips.
