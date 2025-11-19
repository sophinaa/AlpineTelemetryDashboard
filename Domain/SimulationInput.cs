namespace AlpineTelemetryDashboard.Domain;

public class SimulationInput
{
    public double BaseLapTimeSeconds { get; set; }  // e.g. 92.4

    public double FuelDeltaKg { get; set; }         // +ve = more fuel, -ve = less
    public int TyreLaps { get; set; }               // tyre age
    public int WingDeltaClicks { get; set; }        // setup change
    public double ErsModeSecondsGain { get; set; }  // how much ERS gains (in sec)
}
