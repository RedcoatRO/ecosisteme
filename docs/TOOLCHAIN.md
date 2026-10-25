# Toolchain and phase 00.02 — PARTIAL

.NET SDK 10.0.402; libraries target .NET Standard 2.1 / C# 9 for Unity compatibility. Unity Editor pinned to 6000.0.66f2, verified as an official UnityCsReference tag (see tools/toolchain-lock.json). URP 17.0.4 declares Unity 6000.0 compatibility in the official Graphics repository at the recorded commit. This is source metadata evidence, not a successful package restore or Editor launch.

No Unity Editor, Android SDK/NDK, adb or Gradle executable was found in PATH or the inspected standard locations. Java 21 is installed; do not substitute it for Unity's bundled JDK. No Unity credential binding is configured. Unity Personal eligibility and activation must be checked by the developer under current Unity terms; no eligibility is assumed.

UPM access to packages.unity.com and Editor release API services.api.unity.com returned HTTP 403 under the current network policy. No packages-lock.json has been invented. The pinned licensed Editor must resolve Packages/manifest.json and generate the real lock before this checkpoint can pass. Review generated diffs and commit the resolved lock as a coding change. Unity package dependencies are not yet fully pinned transitively.

Android configuration is applied through supported Editor APIs by EcoSim.Editor.CloudBuild.PrepareAndroid. It sets IL2CPP, ARM64 only, landscape-left and minSdk 34. It requires explicit ANDROID_TARGET_SDK and PLAY_POLICY_CHECKED_ON (today's UTC date), rejects a target below 34, and checks that the selected installed Android platform and android.jar exist. Read the current Play target API requirements before setting these variables; the script cannot certify the policy. No targetSdk policy claim is made here. Do not use AndroidApiLevelAuto.

Build pipeline creates generated URP assets and a scene in ignored Assets/Generated; these persist as files but must be regenerated in a fresh checkout. It must not fabricate serialized PlayerSettings. All Editor, rendering, touch and APK checks remain NOT RUN until the missing tooling is supplied.

Sources: https://github.com/Unity-Technologies/UnityCsReference/tree/6000.0.66f2 ; https://github.com/Unity-Technologies/Graphics/tree/feb4de2d9a93a4ae10d287d3a6d7003d08ea3e53/Packages/com.unity.render-pipelines.universal ; https://support.google.com/googleplay/android-developer/answer/11926878 ; https://unity.com/legal/terms-of-service/software
