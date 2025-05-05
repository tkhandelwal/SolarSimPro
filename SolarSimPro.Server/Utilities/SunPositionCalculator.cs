// Utilities/SunPositionCalculator.cs
using System;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Utilities
{
    public static class SunPositionCalculator
    {
        /// <summary>
        /// Calculates the sun position (altitude and azimuth) for the given location and time
        /// </summary>
        public static SunPosition Calculate(double latitude, double longitude, DateTime dateTime)
        {
            // Convert latitude and longitude to radians
            double latRad = DegreesToRadians(latitude);

            // Calculate day of year
            int dayOfYear = dateTime.DayOfYear;

            // Calculate declination angle (in radians)
            double declination = DegreesToRadians(23.45 * Math.Sin(DegreesToRadians(360.0 * (284 + dayOfYear) / 365.0)));

            // Calculate hour angle (in radians)
            double hourAngle = GetHourAngle(dateTime, longitude);

            // Calculate solar zenith angle (in radians)
            double cosZenith = Math.Sin(latRad) * Math.Sin(declination) +
                              Math.Cos(latRad) * Math.Cos(declination) * Math.Cos(hourAngle);

            double zenith = Math.Acos(Math.Max(-1, Math.Min(1, cosZenith)));

            // Calculate solar altitude angle (in degrees)
            double altitude = 90 - RadiansToDegrees(zenith);

            // Calculate solar azimuth angle (in degrees)
            double azimuth = RadiansToDegrees(Math.Atan2(
                -Math.Sin(hourAngle) * Math.Cos(declination),
                Math.Cos(latRad) * Math.Sin(declination) - Math.Sin(latRad) * Math.Cos(declination) * Math.Cos(hourAngle)
            ));

            // Adjust azimuth to be 0-360 degrees
            azimuth = (azimuth + 180) % 360;

            return new SunPosition
            {
                Altitude = altitude,
                Azimuth = azimuth,
                X = Math.Sin(DegreesToRadians(azimuth)) * Math.Cos(DegreesToRadians(altitude)),
                Y = Math.Sin(DegreesToRadians(altitude)),
                Z = Math.Cos(DegreesToRadians(azimuth)) * Math.Cos(DegreesToRadians(altitude))
            };
        }

        private static double GetHourAngle(DateTime dateTime, double longitude)
        {
            // Get the time in hours (with decimals for minutes and seconds)
            double hour = dateTime.Hour + dateTime.Minute / 60.0 + dateTime.Second / 3600.0;

            // Calculate the hour angle (15 degrees per hour from solar noon)
            // Adjust for longitude (4 minutes per degree)
            double solarTime = hour + (longitude / 15.0);

            // Hour angle is 0 at solar noon
            return DegreesToRadians((solarTime - 12) * 15);
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        private static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }
    }

    public class SunPosition
    {
        public double Altitude { get; set; }  // Degrees
        public double Azimuth { get; set; }   // Degrees
        public double X { get; set; }         // Vector component
        public double Y { get; set; }         // Vector component
        public double Z { get; set; }         // Vector component
    }
}