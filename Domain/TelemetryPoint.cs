namespace AlpineTelemetryDashboard.Domain;

public class TelemetryPoint
{
    public double Distance { get; set; }      // m
    public double SpeedKph { get; set; }      // km/h
    public double Throttle { get; set; }      // 0–1
    public double Brake { get; set; }         // 0–1
    public int Gear { get; set; }             // 1–8
    public double Rpm { get; set; }           // engine rpm
}
