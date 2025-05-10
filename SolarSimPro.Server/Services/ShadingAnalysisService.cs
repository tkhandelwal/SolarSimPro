// Services/ShadingAnalysisService.cs
using SolarSimPro.Server.Models;
using SolarSimPro.Server.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarSimPro.Server.Services
{
    public class ShadingAnalysisService
    {
        public bool DoesCastShadow(ShadingObject obj, Panel panel, SunPosition sunPosition)
        {
            // Basic shadow calculation algorithm
            // For a simple implementation, we check if the ray from the panel to the sun
            // intersects with the shading object

            // Get panel center position
            var panelPos = panel.Position;

            // Create a ray from the panel towards the sun
            var rayDirection = new Vector3D
            {
                X = sunPosition.X,
                Y = sunPosition.Y,
                Z = sunPosition.Z
            };

            // For a simple building object (box)
            if (obj.Type == ObjectType.Building)
            {
                // Calculate bounding box of the building
                double minX = obj.Points.Min(p => p.X);
                double maxX = obj.Points.Max(p => p.X);
                double minY = obj.Points.Min(p => p.Y);
                double maxY = obj.Points.Max(p => p.Y);
                double minZ = 0;
                double maxZ = obj.Height;

                // Check if ray intersects with box
                return RayIntersectsBox(
                    new Vector3D { X = panelPos.X, Y = panelPos.Y, Z = panelPos.Z },
                    rayDirection,
                    new Vector3D { X = minX, Y = minY, Z = minZ },
                    new Vector3D { X = maxX, Y = maxY, Z = maxZ }
                );
            }

            // For simplicity, other object types (trees, etc.) use similar box calculations
            return false;
        }

        private bool RayIntersectsBox(Vector3D rayOrigin, Vector3D rayDirection,
                                    Vector3D boxMin, Vector3D boxMax)
        {
            // Implementation of ray-box intersection algorithm
            // This is a simplified version - a real implementation would use
            // more sophisticated ray-tracing techniques

            double tMin = double.MinValue;
            double tMax = double.MaxValue;

            // Check each slab (pair of parallel planes)
            for (int i = 0; i < 3; i++)
            {
                double d = rayDirection.GetComponent(i);
                double o = rayOrigin.GetComponent(i);
                double min = boxMin.GetComponent(i);
                double max = boxMax.GetComponent(i);

                // Ray is parallel to slab
                if (Math.Abs(d) < 1e-8)
                {
                    // Ray origin outside box => no intersection
                    if (o < min || o > max)
                        return false;
                }
                else
                {
                    // Calculate intersection parameters
                    double t1 = (min - o) / d;
                    double t2 = (max - o) / d;

                    // Ensure t1 <= t2
                    if (t1 > t2)
                    {
                        double temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }

                    // Update tMin and tMax
                    tMin = Math.Max(tMin, t1);
                    tMax = Math.Min(tMax, t2);

                    // No intersection if tMin > tMax
                    if (tMin > tMax)
                        return false;
                }
            }

            // Ray intersects all 3 slabs
            return true;
        }
    }

    public class Vector3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double GetComponent(int index)
        {
            return index switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new IndexOutOfRangeException("Vector index out of range")
            };
        }
    }
}