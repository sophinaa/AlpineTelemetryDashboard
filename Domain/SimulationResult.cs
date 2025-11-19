namespace AlpineTelemetryDashboard.Domain;

public class SimulationResult
{
    public double BaseLapTimeSeconds { get; set; }
    public double SimulatedLapTimeSeconds { get; set; }
    public double DeltaSeconds => SimulatedLapTimeSeconds - BaseLapTimeSeconds;
}
