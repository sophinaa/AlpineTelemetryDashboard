using System.Text.Json;
using AlpineTelemetryDashboard.Domain;
using AlpineTelemetryDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlpineTelemetryDashboard.Pages;

public class IndexModel : PageModel
{
    private readonly TelemetryLoader _loader;
    private readonly PaceSimulator _simulator;

    public IndexModel(TelemetryLoader loader, PaceSimulator simulator)
    {
        _loader = loader;
        _simulator = simulator;
    }

    [BindProperty]
    public IFormFile? TelemetryFile { get; set; }

    public string? TelemetryJson { get; set; }
    public double? EstimatedLapTimeSeconds { get; set; }

    [BindProperty]
    public SimulationInput Input { get; set; } = new();

    public SimulationResult? Result { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostUploadAsync()
    {
        if (TelemetryFile == null || TelemetryFile.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Please upload a CSV telemetry file.");
            return Page();
        }

        await using var stream = TelemetryFile.OpenReadStream();
        var lap = _loader.LoadFromCsv("Lap 1", stream);
        EstimatedLapTimeSeconds = lap.LapTimeSeconds;

        // minimal data for chart
        var data = lap.Points.Select(p => new
        {
            p.Distance,
            p.SpeedKph
        });

        TelemetryJson = JsonSerializer.Serialize(data);

        return Page();
    }

    public IActionResult OnPostSimulate()
    {
        if (Input.BaseLapTimeSeconds <= 0)
        {
            ModelState.AddModelError(string.Empty, "Base lap time must be > 0.");
            return Page();
        }

        Result = _simulator.Simulate(Input);
        return Page();
    }
}
