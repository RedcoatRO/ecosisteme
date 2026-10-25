# Specificație — PROVIZORIU

Document inițial: contractul furnizat de beneficiar, reprodus mai jos. Promptul fazei 00 a fost primit ulterior; rezultatele și limitele sunt în reports/PHASE_00_REPORT.md. Contractul de mai jos rămâne neschimbat.

# JOC ECOSISTEME — CONTRACTUL GLOBAL PENTRU CODEX

## Mandat
Acționează ca arhitect principal Unity/C#, inginer al simulării, dezvoltator Android, QA lead și technical writer. Construiește incremental un simulator ecologic 3D stilizat pentru Android, cu nucleu ecologic independent de Unity, gameplay offline-first, RO/EN și o campanie educațională plus sandbox.

## Constrângeri confirmate de beneficiar
- Dezvoltator unic asistat de Codex, experiență profesională Unity neasumată; **buget inițial 0 €**; numai instrumente și resurse gratuite/licențiate compatibil cu utilizarea comercială.
- Unity 6 + C# + URP; Android exclusiv, landscape, Android 14+ (`minSdk` 34), ARM64; `targetSdk` conform politicii Google Play actuale la build/lansare.
- Primul telefon fizic de referință: Xiaomi 12T Pro; dispozitive suplimentare ulterior. Nu garanta performanță neverificată.
- Simulare autonomă hibridă indivizi/cohorte/populații; plante, vertebrate, nevertebrate, ciuperci, microorganisme; genetică, boli, migrație, descompunere modulare; procese vizibile real în grafică, nu decorative.
- P0: 6–10 specii/grupuri funcționale, circuit resurse-plante-consumatori-descompunere-reproducere-mortalitate, interacțiune, accelerare, statistici și salvare.
- P1: două habitate interconectate, 3–5 misiuni, sandbox limitat, finisaj vizual reprezentativ.
- P2: insulă mixtă pădure/pajiște/apă dulce, 30–50 de **specii biologic identificate** (grupurile funcționale nu contează), campanie, sandbox, catalog verificabil, genetică și boli limitate; speciația avansată și preistoricul ulterior.
- Graphical detail adaptiv Low/Medium/High/Ultra; 30 FPS sustenabil vizat pentru dispozitive acceptate și 60 FPS opțional unde hardware permite.
- Multiple profiluri locale și salvări; protecții copii; cont neobligatoriu; nicio reclamă; bază gratuită, extinderi plătite opționale ulterior; gameplay instalat complet offline; telemetrie numai opt-in.
- Fiecare subfază necesită dovadă verificabilă, teste, documentație, criterii de acceptare; nu modifica cerințele acceptate unilateral.

## Ordin de execuție
Folosește **un prompt de fază** separat din pachet, în ordine `00`–`17`. Fiecare prompt are **8 subfaze**. Asta înseamnă 18 prompturi principale și 144 checkpoint-uri, NU 114 prompturi. Într-o sesiune, execută subfazele în ordine până la un blocaj real sau limită de context; raportează exact care au fost efectiv încheiate. După acceptarea fazei se trece la următoarea. Nu genera toate 18 fazele simultan.

## Politică de livrare
Codul complet, testele și structura proiectului trebuie să fie în repository-ul utilizatorului; rezumatele nu înlocuiesc fișierele. Nu inventa fișiere, screenshot-uri, builduri sau rezultate. Pentru acțiuni externe cu cost sau publicare cere acceptare explicită.

## Reguli de execuție obligatorii
1. Lucrezi exclusiv în repository-ul disponibil. Citește mai întâi `AGENTS.md`, `docs/MASTER_SPEC.md`, `docs/ARCHITECTURE.md`, `docs/ECOLOGY.md`, `docs/ROADMAP.md`, rapoartele și codul existent. Dacă un document lipsește, creează doar o versiune minimală marcată explicit ca provizorie, fără a inventa decizii ale beneficiarului.
2. Nu rescrie proiectul și nu rupe API-urile stabilite. Verifică dependințele fazelor și compatibilitatea salvărilor înainte de modificare. Fă modificări atomice, bine delimitate. Nu executa `git reset --hard`, ștergeri de date sau comenzi destructive.
3. Rulează subfazele în ordinea indicată. Pentru fiecare: (a) inspectează situația actuală, (b) definește planul și contractele, (c) implementează codul și datele, (d) adaugă teste, (e) execută testele disponibile, (f) documentează rezultatul. Dacă o subfază e blocată, nu pretinde că faza este completă; marchează `BLOCKED` sau `PARTIAL` și furnizează remediul și comanda concretă.
4. Nu afirma că Unity, Android SDK, Gradle, un telefon, o licență, un build, un test sau un asset există fără verificare. Dacă lipsesc, pregătește scripturile și raportează verificările care trebuie efectuate local.
5. Nu introduce API-uri plătite, asset-uri fără licență comercială explicită, dependențe SaaS obligatorii, cont obligatoriu sau colectare de date personale. Orice resursă externă trebuie să aibă fișă cu sursă, autor, licență și condiții de atribuire.
6. `EcoSim.Core` nu importă `UnityEngine`. Comportamentul ecologic este guvernat de starea simulării, nu de numărul de obiecte randate. Folosește ID-uri stabile, semințe PRNG explicite, ordine deterministă a evenimentelor și reprezentări agregate când este cazul.
7. Ținta este numai Android, landscape, Android 14+ (`minSdk` 34); versiunea `targetSdk` se validează conform cerinței Google Play aplicabile la momentul buildului. Grafică stilizată 3D URP, UI RO/EN, offline-first.
8. Orice rezultat numeric biologic este fie documentat cu proveniență/unități, fie etichetat ca ipoteză de model. Nu fabrica informații științifice, date de test sau rezultate de performanță.
9. La final scrie `reports/PHASE_XX_REPORT.md` cu: status pe subfază, fișiere create/modificate, comenzile exact executate, rezultate și dovezi, teste lipsă, riscuri, buguri, criterii de acceptare trecute/eșuate, pași manuali, următoarea acțiune sigură. Actualizează `docs/DECISIONS.md` pentru alegeri tehnice, fără a modifica cerințele de produs.
10. Dacă nu poți rezolva un blocaj fără informații de la mine, finalizează toate părțile independente și pune maximum 3 întrebări strict blocante, într-un raport separat. Nu prezenta intenții ca livrabile.
