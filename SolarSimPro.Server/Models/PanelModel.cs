// Models/PanelModel.cs
public class PanelModel
{
    public Guid Id { get; set; }
    public string Manufacturer { get; set; }
    public string ModelName { get; set; }
    public double NominalPowerWp { get; set; }
    public double Efficiency { get; set; }
    public double Width { get; set; }   // in meters
    public double Height { get; set; }  // in meters
    public double Thickness { get; set; } // in meters
    public double Weight { get; set; }  // in kg

    // Electrical characteristics
    public double VocStc { get; set; }  // Open circuit voltage at STC
    public double IscStc { get; set; }  // Short circuit current at STC
    public double VmppStc { get; set; } // Voltage at maximum power point at STC
    public double ImppStc { get; set; } // Current at maximum power point at STC

    // Temperature coefficients
    public double TempCoeffPmax { get; set; } // %/°C
    public double TempCoeffVoc { get; set; }  // %/°C
    public double TempCoeffIsc { get; set; }  // %/°C
}