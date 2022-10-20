using MeasuringStations.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations.Services
{
    internal class JsonStationSaver : SimpleFormatStationSaver
    {
        protected override string GetSerialized(StationDetails station)
        {
            return JsonConvert.SerializeObject(station);
        }
    }
}
