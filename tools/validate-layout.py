#!/usr/bin/env python3
"""Input: repository root. Output: missing boundaries or invalid engine references; nonzero on error."""
import json
from pathlib import Path
import sys

def validate(root):
    errors = []
    for name in ('unity-client', 'src/EcoSim.Core', 'src/EcoSim.Gameplay', 'src/EcoSim.Contracts', 'tests', 'tools', 'docs', 'content'):
        if not (root / name).is_dir():
            errors.append('Missing directory: ' + name)
    for name in ('Contracts', 'Core', 'Gameplay'):
        assembly = root / f'src/EcoSim.{name}/EcoSim.{name}.asmdef'
        if not assembly.is_file():
            errors.append('Missing assembly: ' + name)
            continue
        data = json.loads(assembly.read_text())
        if data.get('noEngineReferences') is not True:
            errors.append('Engine reference boundary disabled: ' + name)
        if any(x.startswith('Unity') for x in data.get('references', [])):
            errors.append('Engine assembly reference: ' + name)
    for source in (root / 'src').rglob('*.cs'):
        if 'UnityEngine' in source.read_text():
            errors.append('Engine dependency: ' + str(source.relative_to(root)))
    return errors

if __name__ == '__main__':
    errors = validate(Path(sys.argv[1] if len(sys.argv) > 1 else '.'))
    print('\n'.join(errors) if errors else 'PASS: repository layout and engine boundaries')
    sys.exit(bool(errors))
