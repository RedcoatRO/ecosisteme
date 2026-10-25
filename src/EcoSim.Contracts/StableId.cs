using System;
using System.Globalization;

namespace EcoSim.Contracts
{
    /// <summary>Nonzero identity allocated by simulation, never by renderer instance IDs.</summary>
    public readonly struct StableId : IEquatable<StableId>
    {
        public ulong Value { get; }
        public StableId(ulong value)
        {
            if (value == 0) throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
        }
        public static StableId Parse(string text)
        {
            if (!ulong.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out var value) || value == 0)
                throw new FormatException("Stable ID must be a nonzero unsigned decimal integer.");
            return new StableId(value);
        }
        public bool Equals(StableId other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is StableId id && Equals(id);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
    }
}
