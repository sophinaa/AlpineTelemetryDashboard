using System.Collections.Generic;

namespace AlpineTelemetryDashboard.Domain;

public class LapTelemetry
{
    public string LapName { get; set; } = "";
    public double LapTimeSeconds { get; set; }
    public IList<TelemetryPoint> Points { get; set; } = new List<TelemetryPoint>();
}
