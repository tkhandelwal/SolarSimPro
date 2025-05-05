// Install the PdfSharp NuGet package first
// Install-Package PdfSharp -Version 1.50.5147
// Models/PdfModels.cs
using System;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace SolarSimPro.Server.Models
{
    // PdfSharp-related classes
    public class PageSize
    {
        public static readonly PageSize A4 = new PageSize(595, 842); // Points (1/72 inch)

        public double Width { get; }
        public double Height { get; }

        public PageSize(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}