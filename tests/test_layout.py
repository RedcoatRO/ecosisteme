import importlib.util
from pathlib import Path
import shutil
import tempfile
import unittest
ROOT = Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location('layout', ROOT / 'tools/validate-layout.py')
layout = importlib.util.module_from_spec(spec)
spec.loader.exec_module(layout)

class LayoutTests(unittest.TestCase):
    def test_repository_boundaries(self):
        self.assertEqual([], layout.validate(ROOT))

    def test_missing_core_is_rejected(self):
        with tempfile.TemporaryDirectory() as folder:
            target = Path(folder)
            shutil.copytree(ROOT / 'src', target / 'src')
            shutil.rmtree(target / 'src/EcoSim.Core')
            self.assertIn('Missing directory: src/EcoSim.Core', layout.validate(target))

    def test_engine_reference_is_rejected(self):
        with tempfile.TemporaryDirectory() as folder:
            target = Path(folder)
            shutil.copytree(ROOT / 'src', target / 'src')
            (target / 'src/EcoSim.Core/Invalid.cs').write_text('using UnityEngine;')
            self.assertTrue(any('Engine dependency' in x for x in layout.validate(target)))

    def test_no_dotnet_outputs_inside_unity_packages(self):
        for name in ('Contracts', 'Core', 'Gameplay'):
            package = ROOT / f'src/EcoSim.{name}'
            self.assertFalse((package / 'obj').exists())
            self.assertFalse((package / 'bin').exists())
