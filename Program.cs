using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Gebruik poort uit environment (Dokploy/containers), fallback naar 8080
var portEnv = Environment.GetEnvironmentVariable("PORT");
var port = int.TryParse(portEnv, out var p) ? p : 8080;

// Luister op alle interfaces zodat het vanuit de container bereikbaar is
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, port);
});

var app = builder.Build();

app.MapGet("/", () => Results.Json(new
{
    name = "DokployApp",
    status = "ok",
    timestamp = DateTimeOffset.UtcNow
}));

app.MapGet("/health", () => Results.Text("OK"));

app.Run();
