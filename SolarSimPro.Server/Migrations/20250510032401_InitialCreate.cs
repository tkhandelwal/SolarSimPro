using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarSimPro.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InverterModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NominalPowerAc = table.Column<double>(type: "REAL", nullable: false),
                    MaxEfficiency = table.Column<double>(type: "REAL", nullable: false),
                    MinInputVoltage = table.Column<double>(type: "REAL", nullable: false),
                    MaxInputVoltage = table.Column<double>(type: "REAL", nullable: false),
                    MaxInputCurrent = table.Column<double>(type: "REAL", nullable: false),
                    NumberOfMpptInputs = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InverterModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PanelModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NominalPowerWp = table.Column<double>(type: "REAL", nullable: false),
                    Efficiency = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<double>(type: "REAL", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Thickness = table.Column<double>(type: "REAL", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    VocStc = table.Column<double>(type: "REAL", nullable: false),
                    IscStc = table.Column<double>(type: "REAL", nullable: false),
                    VmppStc = table.Column<double>(type: "REAL", nullable: false),
                    ImppStc = table.Column<double>(type: "REAL", nullable: false),
                    TempCoeffPmax = table.Column<double>(type: "REAL", nullable: false),
                    TempCoeffVoc = table.Column<double>(type: "REAL", nullable: false),
                    TempCoeffIsc = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanelModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ClientName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Altitude = table.Column<double>(type: "REAL", nullable: false),
                    TimeZone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Albedo = table.Column<double>(type: "REAL", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolarSystems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TotalCapacityKWp = table.Column<double>(type: "REAL", nullable: false),
                    NumberOfModules = table.Column<int>(type: "INTEGER", nullable: false),
                    ModuleArea = table.Column<double>(type: "REAL", nullable: false),
                    Tilt = table.Column<double>(type: "REAL", nullable: false),
                    Azimuth = table.Column<double>(type: "REAL", nullable: false),
                    PanelModelId = table.Column<Guid>(type: "TEXT", nullable: true),
                    InverterModelId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolarSystems_InverterModels_InverterModelId",
                        column: x => x.InverterModelId,
                        principalTable: "InverterModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SolarSystems_PanelModels_PanelModelId",
                        column: x => x.PanelModelId,
                        principalTable: "PanelModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SolarSystems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inverters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SolarSystemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    InverterModelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StringsPerMppt = table.Column<int>(type: "INTEGER", nullable: false),
                    ModulesPerString = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inverters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inverters_InverterModels_InverterModelId",
                        column: x => x.InverterModelId,
                        principalTable: "InverterModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inverters_SolarSystems_SolarSystemId",
                        column: x => x.SolarSystemId,
                        principalTable: "SolarSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimulationResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SolarSystemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SimulationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AnnualProduction = table.Column<double>(type: "REAL", nullable: false),
                    SpecificProduction = table.Column<double>(type: "REAL", nullable: false),
                    PerformanceRatio = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimulationResults_SolarSystems_SolarSystemId",
                        column: x => x.SolarSystemId,
                        principalTable: "SolarSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAnalyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SimulationResultId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TotalSystemCost = table.Column<double>(type: "REAL", nullable: false),
                    AnnualSavingsYear1 = table.Column<double>(type: "REAL", nullable: false),
                    SimplePaybackPeriod = table.Column<double>(type: "REAL", nullable: false),
                    NPV = table.Column<double>(type: "REAL", nullable: false),
                    ROI = table.Column<double>(type: "REAL", nullable: false),
                    LCOE = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAnalyses_SimulationResults_SimulationResultId",
                        column: x => x.SimulationResultId,
                        principalTable: "SimulationResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LossBreakdowns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SimulationResultId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IAMLoss = table.Column<double>(type: "REAL", nullable: false),
                    SoilingLoss = table.Column<double>(type: "REAL", nullable: false),
                    IrradianceLoss = table.Column<double>(type: "REAL", nullable: false),
                    TemperatureLoss = table.Column<double>(type: "REAL", nullable: false),
                    ModuleQualityLoss = table.Column<double>(type: "REAL", nullable: false),
                    MismatchLoss = table.Column<double>(type: "REAL", nullable: false),
                    OhmicWiringLoss = table.Column<double>(type: "REAL", nullable: false),
                    InverterEfficiencyLoss = table.Column<double>(type: "REAL", nullable: false),
                    ACOhmicLoss = table.Column<double>(type: "REAL", nullable: false),
                    SystemUnavailabilityLoss = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossBreakdowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LossBreakdowns_SimulationResults_SimulationResultId",
                        column: x => x.SimulationResultId,
                        principalTable: "SimulationResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SimulationResultId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    GlobHor = table.Column<double>(type: "REAL", nullable: false),
                    DiffHor = table.Column<double>(type: "REAL", nullable: false),
                    Temperature = table.Column<double>(type: "REAL", nullable: false),
                    GlobInc = table.Column<double>(type: "REAL", nullable: false),
                    GlobEff = table.Column<double>(type: "REAL", nullable: false),
                    EArray = table.Column<double>(type: "REAL", nullable: false),
                    EGrid = table.Column<double>(type: "REAL", nullable: false),
                    PR = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyResults_SimulationResults_SimulationResultId",
                        column: x => x.SimulationResultId,
                        principalTable: "SimulationResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAnalyses_SimulationResultId",
                table: "FinancialAnalyses",
                column: "SimulationResultId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inverters_InverterModelId",
                table: "Inverters",
                column: "InverterModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Inverters_SolarSystemId",
                table: "Inverters",
                column: "SolarSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_LossBreakdowns_SimulationResultId",
                table: "LossBreakdowns",
                column: "SimulationResultId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyResults_SimulationResultId",
                table: "MonthlyResults",
                column: "SimulationResultId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationResults_SolarSystemId",
                table: "SimulationResults",
                column: "SolarSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_SolarSystems_InverterModelId",
                table: "SolarSystems",
                column: "InverterModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SolarSystems_PanelModelId",
                table: "SolarSystems",
                column: "PanelModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SolarSystems_ProjectId",
                table: "SolarSystems",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialAnalyses");

            migrationBuilder.DropTable(
                name: "Inverters");

            migrationBuilder.DropTable(
                name: "LossBreakdowns");

            migrationBuilder.DropTable(
                name: "MonthlyResults");

            migrationBuilder.DropTable(
                name: "SimulationResults");

            migrationBuilder.DropTable(
                name: "SolarSystems");

            migrationBuilder.DropTable(
                name: "InverterModels");

            migrationBuilder.DropTable(
                name: "PanelModels");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
