using MeasuringStations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MeasuringStations.Services
{
    internal class XmlStationSaver : SimpleFormatStationSaver
    {
        private readonly XmlSerializer _serializer;

        public XmlStationSaver()
        {
            _serializer = new(typeof(StationDetails));
        }

        protected override string GetSerialized(StationDetails station)
        {
            using var stringWriter = new StringWriter();
            using var xmlWriter = new XmlTextWriter(stringWriter);
            _serializer.Serialize(xmlWriter, station);
            return stringWriter.ToString();
        }
    }
}
