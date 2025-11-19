using AlpineTelemetryDashboard.Domain;

namespace AlpineTelemetryDashboard.Services;

public class TelemetryLoader
{
    // CSV format:
    // distance,speed_kph,throttle,brake,gear,rpm
    public LapTelemetry LoadFromCsv(string lapName, Stream csvStream)
    {
        var points = new List<TelemetryPoint>();

        using var reader = new StreamReader(csvStream);
        string? line;
        bool isHeader = true;

        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (isHeader)
            {
                isHeader = false; // skip header
                continue;
            }

            var parts = line.Split(',');
            if (parts.Length < 6) continue;

            try
            {
                points.Add(new TelemetryPoint
                {
                    Distance = double.Parse(parts[0]),
                    SpeedKph = double.Parse(parts[1]),
                    Throttle = double.Parse(parts[2]),
                    Brake = double.Parse(parts[3]),
                    Gear = int.Parse(parts[4]),
                    Rpm = double.Parse(parts[5])
                });
            }
            catch
            {
                // skip bad rows
            }
        }

        double avgSpeedMps = points.Any() ? points.Average(p => p.SpeedKph) / 3.6 : 0;
        double totalDistance = points.LastOrDefault()?.Distance ?? 0;
        double lapTime = avgSpeedMps > 0 ? totalDistance / avgSpeedMps : 0;

        return new LapTelemetry
        {
            LapName = lapName,
            LapTimeSeconds = lapTime,
            Points = points
        };
    }
}
