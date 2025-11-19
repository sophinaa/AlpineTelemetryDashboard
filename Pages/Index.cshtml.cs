using System.Text.Json;
using AlpineTelemetryDashboard.Domain;
using AlpineTelemetryDashboard.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlpineTelemetryDashboard.Pages;

public class IndexModel : PageModel
{
    private readonly TelemetryLoader _loader;
    private readonly PaceSimulator _simulator;
    private readonly IWebHostEnvironment _env;

    public IndexModel(TelemetryLoader loader, PaceSimulator simulator, IWebHostEnvironment env)
    {
        _loader = loader;
        _simulator = simulator;
        _env = env;
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

    public IActionResult OnPostLoadSample()
    {
        var path = Path.Combine(_env.WebRootPath, "data", "sample_lap.csv");
        if (!System.IO.File.Exists(path))
        {
            ModelState.AddModelError(string.Empty, "Sample telemetry file not found.");
            return Page();
        }

        using var stream = System.IO.File.OpenRead(path);
        var lap = _loader.LoadFromCsv("Sample Lap", stream);
        EstimatedLapTimeSeconds = lap.LapTimeSeconds;

        var data = lap.Points.Select(p => new
        {
            p.Distance,
            p.SpeedKph,
            p.Throttle,
            p.Brake
        });

        TelemetryJson = JsonSerializer.Serialize(data);
        return Page();
    }
}
