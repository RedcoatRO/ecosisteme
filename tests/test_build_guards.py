"""Tests shell fail-fast behavior only. They do not emulate or validate Unity."""
import os
from pathlib import Path
import subprocess
import unittest
ROOT = Path(__file__).resolve().parents[1]

class BuildGuards(unittest.TestCase):
    def test_missing_editor_fails_clearly(self):
        env = {**os.environ, 'UNITY_EDITOR': '/definitely/missing/unity'}
        result = subprocess.run(['bash', 'tools/unity-build.sh'], cwd=ROOT, env=env, capture_output=True, text=True)
        self.assertEqual(2, result.returncode)
        self.assertIn('BLOCKED: set UNITY_EDITOR', result.stderr)

    def test_invalid_target_stops_before_editor_launch(self):
        # /bin/false cannot build: reaching it would return 1, rather than guard exit 2.
        env = {**os.environ, 'UNITY_EDITOR': '/bin/false', 'ANDROID_TARGET_SDK': '33'}
        result = subprocess.run(['bash', 'tools/unity-build.sh'], cwd=ROOT, env=env, capture_output=True, text=True)
        self.assertEqual(2, result.returncode)
        self.assertIn('ANDROID_TARGET_SDK', result.stderr)
