using MeasuringStations.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations.Services
{
    internal class StationFileSaverFactory : IStationFileSaverFactory
    {
        public IStationFileSaver Create(string extension)
        {
            switch (extension)
            {
                case "json":
                    return new JsonStationSaver();
                case "xml":
                    return new XmlStationSaver();
                case "pdf":
                    return new PdfStationSaver();
                default:
                    throw new NotSupportedExtensionException();
            }
        }
    }
}
