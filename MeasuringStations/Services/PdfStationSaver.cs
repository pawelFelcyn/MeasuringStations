using MeasuringStations.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MeasuringStations.Services
{
    internal class PdfStationSaver : IStationFileSaver
    {
        public double LineSpacing { get; set; } = 2;
        public double FontSize { get; set; } = 16;

        public PdfStationSaver()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public Task SaveAsync(StationDetails station, string path)
        {
            var document = new PdfDocument();

            var font = new XFont("Arial", FontSize);
            var page = document.AddPage();
            var graph = XGraphics.FromPdfPage(page);

            var properties = typeof(StationDetails).GetProperties();
            int writtenLines = 0;
            foreach (var prop in properties)
            {
                AddProp(page, graph, font, station, prop, writtenLines);
                writtenLines++;
            }

            document.Save(path);
            return Task.CompletedTask;
        }

        private void AddProp(PdfPage page, XGraphics graph, XFont font, 
            StationDetails station, PropertyInfo prop, int writtenLines)
        {
            var @string = $"{prop.Name}:      {prop.GetValue(station)}";
            double y = writtenLines * font.Height + writtenLines * LineSpacing; 
            graph.DrawString(@string, font, XBrushes.Black, new XRect(0, y, page.Width.Point, page.Height.Point),
                XStringFormats.TopCenter);
        }
    }
}
