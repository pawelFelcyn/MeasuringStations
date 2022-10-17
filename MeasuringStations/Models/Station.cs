using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string StationsName { get; set; }
        public decimal GegrLat { get; set; }
        public decimal GegrLon { get; set; }
        public City City { get; set; }
        public string AddressStreet { get; set; }
    }
}
