// Services/ShadingAnalysisService.cs
public class ShadingAnalysisService
{
    public ShadingResult AnalyzeShadingImpact(List<Panel> panels, List<ShadingObject> objects, GeoCoordinates location)
    {
        var result = new ShadingResult();

        // For each month and hour of the day
        for (int month = 1; month <= 12; month++)
        {
            for (int hour = 6; hour <= 18; hour++)
            {
                // Calculate sun position (altitude, azimuth) for this date/time
                var sunPosition = SunPositionCalculator.Calculate(
                    location.Latitude,
                    location.Longitude,
                    new DateTime(DateTime.Now.Year, month, 15, hour, 0, 0)
                );

                // For each panel, check if it's shaded
                foreach (var panel in panels)
                {
                    bool isShaded = false;

                    // Check each object if it casts shadow on this panel
                    foreach (var obj in objects)
                    {
                        if (DoesCastShadow(obj, panel, sunPosition))
                        {
                            isShaded = true;
                            break;
                        }
                    }

                    // Record shading for this panel at this time
                    result.AddShadingData(panel.Id, month, hour, isShaded);
                }
            }
        }

        return result;
    }
}