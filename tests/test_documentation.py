import re
from pathlib import Path
import tempfile
import unittest
ROOT = Path(__file__).resolve().parents[1]

def broken_links(document):
    result = []
    for link in re.findall(r'\]\(([^)]+)\)', document.read_text()):
        if '://' in link or link.startswith('#'):
            continue
        if not (document.parent / link.split('#')[0]).exists():
            result.append(link)
    return result

class DocumentationTests(unittest.TestCase):
    def test_readme_links_resolve(self):
        self.assertEqual([], broken_links(ROOT / 'README.md'))

    def test_missing_local_document_is_detected(self):
        with tempfile.TemporaryDirectory() as folder:
            document = Path(folder) / 'README.md'
            document.write_text('[missing](docs/absent.md)')
            self.assertEqual(['docs/absent.md'], broken_links(document))
