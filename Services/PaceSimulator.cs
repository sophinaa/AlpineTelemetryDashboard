using AlpineTelemetryDashboard.Domain;

namespace AlpineTelemetryDashboard.Services;

public class PaceSimulator
{
    // Crude but explainable coefficients.
    private const double FuelSecondsPerKg = 0.035;     // ~0.03–0.04s per kg
    private const double TyreSecondsPerLap = 0.02;     // per lap of wear
    private const double WingSecondsPerClick = 0.015;  // aero trade-off

    public SimulationResult Simulate(SimulationInput input)
    {
        double time = input.BaseLapTimeSeconds;

        time += input.FuelDeltaKg * FuelSecondsPerKg;
        time += input.TyreLaps * TyreSecondsPerLap;
        time += input.WingDeltaClicks * WingSecondsPerClick;
        time -= input.ErsModeSecondsGain; // ERS gives us time

        return new SimulationResult
        {
            BaseLapTimeSeconds = input.BaseLapTimeSeconds,
            SimulatedLapTimeSeconds = time
        };
    }
}
