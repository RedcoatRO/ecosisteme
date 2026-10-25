using System;
using EcoSim.Contracts;

namespace EcoSim.Core
{
    public readonly struct WorldSnapshot
    {
        public StableId ProbeId { get; }
        public long Tick { get; }
        public double ElapsedSeconds { get; }
        public uint DiagnosticSample { get; }
        public double ProbeXMeters { get; }
        public double ProbeZMeters { get; }
        internal WorldSnapshot(long tick, double elapsed, uint sample, double x, double z)
        {
            ProbeId = new StableId(1); Tick = tick; ElapsedSeconds = elapsed;
            DiagnosticSample = sample; ProbeXMeters = x; ProbeZMeters = z;
        }
    }

    /// <summary>Phase-00 deterministic clock and input probe. No ecological dynamics yet.</summary>
    public sealed class SimulationStep
    {
        private readonly WorldConfig config;
        private uint randomState;
        public WorldSnapshot Snapshot { get; private set; }
        public SimulationStep(WorldConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            randomState = config.Seed;
            Snapshot = new WorldSnapshot(0, 0, 0, 0, 0);
        }
        public WorldSnapshot Advance()
        {
            long tick = checked(Snapshot.Tick + 1);
            // Explicit unsigned wrap semantics shared by .NET and Unity.
            unchecked
            {
                randomState ^= randomState << 13;
                randomState ^= randomState >> 17;
                randomState ^= randomState << 5;
            }
            Snapshot = new WorldSnapshot(tick, tick * config.StepSeconds, randomState,
                Snapshot.ProbeXMeters, Snapshot.ProbeZMeters);
            return Snapshot;
        }
        public void MoveProbe(double xMeters, double zMeters)
        {
            if (!WorldConfig.Finite(xMeters) || !WorldConfig.Finite(zMeters) ||
                Math.Abs(xMeters) > config.HalfExtentMeters || Math.Abs(zMeters) > config.HalfExtentMeters)
                throw new ArgumentOutOfRangeException(nameof(xMeters), "Probe must remain inside world bounds.");
            Snapshot = new WorldSnapshot(Snapshot.Tick, Snapshot.ElapsedSeconds, Snapshot.DiagnosticSample, xMeters, zMeters);
        }
    }
}
