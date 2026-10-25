using EcoSim.Core;

namespace EcoSim.Gameplay
{
    public sealed class SimulationSession
    {
        private readonly SimulationStep simulation;
        public bool Paused { get; private set; }
        public WorldSnapshot Snapshot => simulation.Snapshot;
        public SimulationSession(WorldConfig config) { simulation = new SimulationStep(config); }
        public void SetPaused(bool paused) => Paused = paused;
        public WorldSnapshot Advance() => Paused ? Snapshot : simulation.Advance();
        public void MoveProbe(double xMeters, double zMeters) => simulation.MoveProbe(xMeters, zMeters);
    }
}
