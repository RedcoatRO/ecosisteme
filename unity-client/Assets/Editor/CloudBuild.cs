using System;
using System.IO;
using System.Globalization;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace EcoSim.Editor
{
    public static class CloudBuild
    {
        private const string ScenePath = "Assets/Scenes/Smoke.unity";
        [MenuItem("EcoSim/Prepare Android smoke")]
        public static void PrepareAndroid()
        {
            if (Application.unityVersion != "6000.0.66f2")
                throw new BuildFailedException("Expected Unity 6000.0.66f2, got " + Application.unityVersion);
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android))
                throw new BuildFailedException("Install Android Build Support, SDK/NDK and bundled OpenJDK for the pinned Editor.");
            string targetText = Environment.GetEnvironmentVariable("ANDROID_TARGET_SDK");
            string checkedOn = Environment.GetEnvironmentVariable("PLAY_POLICY_CHECKED_ON");
            if (!int.TryParse(targetText, NumberStyles.None, CultureInfo.InvariantCulture, out int target) || target < 34 ||
                !Enum.IsDefined(typeof(AndroidSdkVersions), target))
                throw new BuildFailedException("Set ANDROID_TARGET_SDK to an explicit >=34 API supported by this Editor after checking current Play policy.");
            if (checkedOn != DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                throw new BuildFailedException("Read current Play target API policy; set PLAY_POLICY_CHECKED_ON to today's UTC date. This is a developer attestation, not automatic policy verification.");
            string sdk = Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
            if (string.IsNullOrWhiteSpace(sdk) || !File.Exists(Path.Combine(sdk, "platforms", "android-" + target, "android.jar")))
                throw new BuildFailedException("ANDROID_SDK_ROOT must point to the installed SDK containing the chosen target platform android.jar.");
            PlayerSettings.companyName = "RedcoatRO";
            PlayerSettings.productName = "EcoSim Foundation";
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "ro.redcoat.ecosisteme");
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions)34;
            PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)target;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.Android.forceInternetPermission = false;
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3 });
            // IMGUI + legacy touch need no external input package for this foundation.
            var settings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
            var input = settings.FindProperty("activeInputHandler");
            if (input == null) throw new BuildFailedException("Pinned Editor has no supported activeInputHandler setting; configure legacy input and retry.");
            input.intValue = 0;
            settings.ApplyModifiedPropertiesWithoutUndo();
            PrepareRenderPipeline();
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            Debug.Log("ECOSIM_CONFIG_READY: ARM64 IL2CPP landscape minSdk=34 targetSdk=" + target);
        }

        private static void PrepareRenderPipeline()
        {
            Directory.CreateDirectory("Assets/Generated/Resources");
            AssetDatabase.Refresh();
            const string rendererPath = "Assets/Generated/SmokeRenderer.asset";
            var renderer = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(rendererPath);
            if (renderer == null)
            {
                renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
                AssetDatabase.CreateAsset(renderer, rendererPath);
            }
            const string pipelinePath = "Assets/Generated/SmokePipeline.asset";
            var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(pipelinePath);
            if (pipeline == null)
            {
                pipeline = UniversalRenderPipelineAsset.Create(renderer);
                AssetDatabase.CreateAsset(pipeline, pipelinePath);
            }
            GraphicsSettings.defaultRenderPipeline = pipeline;
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                QualitySettings.SetQualityLevel(i, false);
                QualitySettings.renderPipeline = pipeline;
            }
            const string materialPath = "Assets/Generated/Resources/SmokeMaterial.mat";
            if (AssetDatabase.LoadAssetAtPath<Material>(materialPath) == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Lit");
                if (shader == null) throw new BuildFailedException("URP Lit shader missing; resolve pinned packages first.");
                AssetDatabase.CreateAsset(new Material(shader), materialPath);
            }
        }

        public static void Android()
        {
            try
            {
                PrepareAndroid();
                string output = Path.GetFullPath("../builds/android/ecosisteme-smoke.apk");
                Directory.CreateDirectory(Path.GetDirectoryName(output));
                EditorUserBuildSettings.buildAppBundle = false;
                var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    scenes = new[] { ScenePath }, locationPathName = output,
                    target = BuildTarget.Android, options = BuildOptions.Development
                });
                if (report.summary.result != BuildResult.Succeeded || !File.Exists(output))
                    throw new BuildFailedException("Android build failed: " + report.summary.result);
                Debug.Log("ECOSIM_APK_BUILT: " + output);
                EditorApplication.Exit(0);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorApplication.Exit(1);
            }
        }
    }
}
