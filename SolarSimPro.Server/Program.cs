// SolarSimPro.Server/Program.cs
using Microsoft.EntityFrameworkCore;
using SolarSimPro.Server.Data;
using SolarSimPro.Server.Services;
using SolarSimPro.Server.Services.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Database context
var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
var dbPath = Path.Join(builder.Environment.ContentRootPath, "SolarDesign.db");

// Configure SQLite with explicit file path
builder.Services.AddDbContext<SolarDesignDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Register HTTP client
builder.Services.AddHttpClient();

// Register services
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IMeteoDataService, MeteoDataService>();
builder.Services.AddScoped<IPanelService, PanelService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISimulationService, SimulationService>();
builder.Services.AddScoped<ShadingAnalysisService>();
builder.Services.AddScoped<PanelLayoutService>();
builder.Services.AddScoped<ProductionCalculationService>();
builder.Services.AddScoped<FinancialAnalysisService>();

// Add API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SolarDesignDbContext>();
        // This will create the database if it doesn't exist
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();