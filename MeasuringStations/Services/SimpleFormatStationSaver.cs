using MeasuringStations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations.Services
{
    internal class SimpleFormatStationSaver : IStationFileSaver
    {
        public async Task SaveAsync(StationDetails station, string path)
        {
            var formatted = GetSerialized(station);
            await File.WriteAllTextAsync(path, formatted);
        }

        protected virtual string GetSerialized(StationDetails station)
        {
            return station.ToString();
        }
    }
}
