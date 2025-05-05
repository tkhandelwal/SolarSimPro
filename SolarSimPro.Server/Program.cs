// Program.cs
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
builder.Services.AddDbContext<SolarDesignDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// Add API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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