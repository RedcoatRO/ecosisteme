using System;
using System.Globalization;
using EcoSim.Core;
using EcoSim.Gameplay;

var session = new SimulationSession(new WorldConfig(42));
session.MoveProbe(2, -3);
for (int i = 0; i < 4; i++) session.Advance();
var snapshot = session.Snapshot;
Console.WriteLine(FormattableString.Invariant($"tick={snapshot.Tick}; elapsed={snapshot.ElapsedSeconds:F1}s; probe={snapshot.ProbeId}; x={snapshot.ProbeXMeters:F1}m; z={snapshot.ProbeZMeters:F1}m; sample={snapshot.DiagnosticSample}"));
if (snapshot.Tick != 4 || snapshot.ElapsedSeconds != 2 || snapshot.ProbeXMeters != 2 || snapshot.ProbeZMeters != -3)
    return 1;
return 0;
