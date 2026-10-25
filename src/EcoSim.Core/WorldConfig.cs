using System;

namespace EcoSim.Core
{
    public sealed class WorldConfig
    {
        public uint Seed { get; }
        public double StepSeconds { get; }
        public double HalfExtentMeters { get; }
        public WorldConfig(uint seed, double stepSeconds = 0.5, double halfExtentMeters = 10)
        {
            if (seed == 0) throw new ArgumentOutOfRangeException(nameof(seed), "xorshift32 seed must be nonzero.");
            if (!Finite(stepSeconds) || stepSeconds <= 0 || stepSeconds > 60)
                throw new ArgumentOutOfRangeException(nameof(stepSeconds));
            if (!Finite(halfExtentMeters) || halfExtentMeters <= 0 || halfExtentMeters > 10000)
                throw new ArgumentOutOfRangeException(nameof(halfExtentMeters));
            Seed = seed; StepSeconds = stepSeconds; HalfExtentMeters = halfExtentMeters;
        }
        internal static bool Finite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);
    }
}
