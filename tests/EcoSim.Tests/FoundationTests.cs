using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using EcoSim.Contracts;
using EcoSim.Core;
using EcoSim.Gameplay;
using Xunit;

namespace EcoSim.Tests
{
    public class FoundationTests
    {
        [Fact]
        public void ClockMatchesFixture()
        {
            using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "fixtures/world-smoke.json")));
            var f = document.RootElement;
            var world = new SimulationStep(new WorldConfig(f.GetProperty("seed").GetUInt32(), f.GetProperty("stepSeconds").GetDouble()));
            Assert.Equal(0, world.Snapshot.Tick);
            for (int i = 0; i < f.GetProperty("steps").GetInt32(); i++) world.Advance();
            Assert.Equal(f.GetProperty("expectedTick").GetInt64(), world.Snapshot.Tick);
            Assert.Equal(f.GetProperty("expectedElapsedSeconds").GetDouble(), world.Snapshot.ElapsedSeconds);
        }
        [Fact]
        public void SameSeedAndOrderedCommandsReplayExactly()
        {
            var a = new SimulationStep(new WorldConfig(42));
            var b = new SimulationStep(new WorldConfig(42));
            for (int i = 0; i < 1000; i++)
            {
                a.MoveProbe(i % 10, -(i % 10)); b.MoveProbe(i % 10, -(i % 10));
                Assert.Equal(a.Advance(), b.Advance());
            }
        }
        [Fact]
        public void XorshiftMatchesKnownVectorAndSeedsDiffer()
        {
            var a = new SimulationStep(new WorldConfig(42));
            var b = new SimulationStep(new WorldConfig(43));
            Assert.Equal(11355432u, a.Advance().DiagnosticSample);
            Assert.NotEqual(a.Snapshot.DiagnosticSample, b.Advance().DiagnosticSample);
        }
        [Theory]
        [InlineData(0)] [InlineData(-1)] [InlineData(60.001)] [InlineData(double.NaN)] [InlineData(double.PositiveInfinity)]
        public void InvalidTimeStepIsRejected(double seconds) => Assert.Throws<ArgumentOutOfRangeException>(() => new WorldConfig(42, seconds));
        [Theory]
        [InlineData(0)] [InlineData(-1)] [InlineData(10001)] [InlineData(double.NaN)] [InlineData(double.NegativeInfinity)]
        public void InvalidExtentIsRejected(double extent) => Assert.Throws<ArgumentOutOfRangeException>(() => new WorldConfig(42, halfExtentMeters: extent));
        [Fact]
        public void NullConfigAndZeroSeedAreRejected()
        {
            Assert.Throws<ArgumentNullException>(() => new SimulationStep(null!));
            Assert.Throws<ArgumentOutOfRangeException>(() => new WorldConfig(0));
        }
        [Fact]
        public void ConfigAcceptsMaximumBounds() => Assert.Equal(60, new WorldConfig(1, 60, 10000).StepSeconds);
        [Fact]
        public void ProbeUsesInclusiveBoundsAndPreservesClock()
        {
            var world = new SimulationStep(new WorldConfig(1));
            world.Advance();
            world.MoveProbe(-10, 10);
            Assert.Equal(-10, world.Snapshot.ProbeXMeters);
            Assert.Equal(10, world.Snapshot.ProbeZMeters);
            Assert.Equal(1, world.Snapshot.Tick);
            Assert.Equal(new StableId(1), world.Snapshot.ProbeId);
        }
        [Theory]
        [InlineData(10.001, 0)] [InlineData(0, -10.001)] [InlineData(double.NaN, 0)] [InlineData(0, double.PositiveInfinity)]
        public void InvalidProbeCommandDoesNotMutateState(double x, double z)
        {
            var world = new SimulationStep(new WorldConfig(1));
            var before = world.Advance();
            Assert.Throws<ArgumentOutOfRangeException>(() => world.MoveProbe(x, z));
            Assert.Equal(before, world.Snapshot);
        }
        [Fact]
        public void PausedSessionDoesNotAdvanceButAcceptsInput()
        {
            var session = new SimulationSession(new WorldConfig(1));
            session.Advance(); session.SetPaused(true);
            var previous = session.Snapshot;
            Assert.Equal(previous, session.Advance());
            session.MoveProbe(2, 3);
            Assert.Equal(2, session.Snapshot.ProbeXMeters);
            session.SetPaused(false);
            Assert.Equal(2, session.Advance().Tick);
        }
        [Theory]
        [InlineData("1")] [InlineData("18446744073709551615")]
        public void StableIdRoundTripsWithoutPrecisionLoss(string text) => Assert.Equal(text, StableId.Parse(text).ToString());
        [Theory]
        [InlineData("0")] [InlineData("-1")] [InlineData("1.5")] [InlineData("18446744073709551616")] [InlineData("")] [InlineData(" 1")]
        public void InvalidIdIsRejected(string text) => Assert.Throws<FormatException>(() => StableId.Parse(text));
        [Fact]
        public void ZeroIdCannotBeConstructed() => Assert.Throws<ArgumentOutOfRangeException>(() => new StableId(0));
        [Fact]
        public void CurrentSaveAndOlderCompatibleMinorAreReadable()
        {
            SaveFormat.RequireReadable(1, new SchemaVersion(0, 1, 0));
            SaveFormat.RequireReadable(1, new SchemaVersion(0, 0, 9));
        }
        [Theory]
        [InlineData(2, 0, 1)] [InlineData(0, 0, 1)] [InlineData(1, 1, 0)] [InlineData(1, 0, 2)]
        public void IncompatibleSaveIsRejected(int format, int major, int minor) => Assert.Throws<NotSupportedException>(() => SaveFormat.RequireReadable(format, new SchemaVersion(major, minor, 0)));
        [Theory]
        [InlineData(-1, 0, 0)] [InlineData(0, -1, 0)] [InlineData(0, 0, -1)]
        public void NegativeSchemaIsRejected(int major, int minor, int patch) => Assert.Throws<ArgumentOutOfRangeException>(() => new SchemaVersion(major, minor, patch));
        [Fact]
        public void AssembliesHaveNoUnityDependency()
        {
            foreach (var assembly in new[] { typeof(WorldConfig).Assembly, typeof(StableId).Assembly, typeof(SimulationSession).Assembly })
                Assert.DoesNotContain(assembly.GetReferencedAssemblies(), a => a.Name!.StartsWith("Unity", StringComparison.Ordinal));
        }
    }
}
