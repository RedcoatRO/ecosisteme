#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
if [[ -z ${UNITY_EDITOR:-} || ! -x "$UNITY_EDITOR" ]]; then
  echo 'BLOCKED: set UNITY_EDITOR to licensed Unity 6000.0.66f2 with Android Build Support.' >&2
  exit 2
fi
if [[ ! ${ANDROID_TARGET_SDK:-} =~ ^[0-9]+$ ]] || (( ANDROID_TARGET_SDK < 34 )); then
  echo 'BLOCKED: set an explicit ANDROID_TARGET_SDK >=34 after checking current Google Play policy.' >&2
  exit 2
fi
if [[ ${PLAY_POLICY_CHECKED_ON:-} != "$(date -u +%F)" ]]; then
  echo 'BLOCKED: current Play target API policy review required; set PLAY_POLICY_CHECKED_ON to today UTC.' >&2
  exit 2
fi
if [[ -z ${ANDROID_SDK_ROOT:-} || ! -f "$ANDROID_SDK_ROOT/platforms/android-$ANDROID_TARGET_SDK/android.jar" ]]; then
  echo 'BLOCKED: selected Android platform android.jar is not installed in ANDROID_SDK_ROOT.' >&2
  exit 2
fi
mkdir -p builds/logs
exec "$UNITY_EDITOR" -batchmode -nographics -buildTarget Android \
  -projectPath "$PWD/unity-client" -executeMethod EcoSim.Editor.CloudBuild.Android \
  -logFile "$PWD/builds/logs/unity-android.log"
