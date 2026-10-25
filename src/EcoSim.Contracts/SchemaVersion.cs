using System;

namespace EcoSim.Contracts
{
    public readonly struct SchemaVersion : IEquatable<SchemaVersion>
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public SchemaVersion(int major, int minor, int patch)
        {
            if (major < 0 || minor < 0 || patch < 0) throw new ArgumentOutOfRangeException(nameof(major));
            Major = major; Minor = minor; Patch = patch;
        }
        public bool CanRead(SchemaVersion stored) => Major == stored.Major && stored.Minor <= Minor;
        public bool Equals(SchemaVersion other) => Major == other.Major && Minor == other.Minor && Patch == other.Patch;
        public override bool Equals(object? obj) => obj is SchemaVersion version && Equals(version);
        public override int GetHashCode() => (Major, Minor, Patch).GetHashCode();
        public override string ToString() => $"{Major}.{Minor}.{Patch}";
    }
    public static class SaveFormat
    {
        public const int CurrentVersion = 1;
        public static readonly SchemaVersion Schema = new SchemaVersion(0, 1, 0);
        public static void RequireReadable(int version, SchemaVersion schema)
        {
            if (version != CurrentVersion || !Schema.CanRead(schema))
                throw new NotSupportedException("Save format/schema requires an explicit migration.");
        }
    }
    public static class Units
    {
        public const string Duration = "s";
        public const string Distance = "m";
        public const string Mass = "kg";
    }
}
