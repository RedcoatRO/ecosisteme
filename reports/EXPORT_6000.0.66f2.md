# Export PHASE 00 — Unity6000.0.66f2

Versiune corectată conform ADR-0005. Verificare efectivă înainte de arhivare: bash tools/verify.sh, exit0;39 teste .NET și8 Python trecute; exemplu CLI tick4, elapsed2.0s, probe1, x2.0m, z-3.0m. Log: reports/evidence/phase00-export-verify.log.

Faza rămâne PARTIAL. Nu este un APK. Editorul Unity, importul UPM, scena și buildul Android nu au fost executate aici. Capturile beneficiarului confirmă local Editor6000.0.66f2, module Android/JDK/SDK/NDK și activare Personal; nu confirmă încă funcționarea proiectului.

Extrage întregul folder Joc-Ecosisteme într-o locație nouă. Păstrează src lângă unity-client: pachetele Unity locale depind de această structură. Nu suprascrie copia veche. În Unity Hub: Projects → Add/Open → alege subfolderul unity-client; folosește6000.0.66f2. La primul import, trimite erorile Console înainte de alte modificări. Comenzile shell de verificare .NET sunt pentru Linux/cloud; nu sunt comenzi PowerShell.
