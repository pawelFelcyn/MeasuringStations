using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations.Models
{

    public class StationDetails
    {
        public int Id { get; set; }
        public DateTime? StCalcDate { get; set; }
        public IndexLevel StIndexLevel { get; set; }
        public DateTime? StSourceDataDate { get; set; }
        public DateTime? So2CalcDate { get; set; }
        public IndexLevel So2IndexLevel { get; set; }
        public DateTime? So2SourceDataDate { get; set; }
        public DateTime? No2CalcDate { get; set; }
        public IndexLevel No2IndexLevel { get; set; }
        public DateTime? No2SourceDataDate { get; set; }
        public DateTime? Pm10CalcDate { get; set; }
        public IndexLevel Pm10IndexLevel { get; set; }
        public DateTime? Pm10SourceDataDate { get; set; }
        public DateTime? Pm25CalcDate { get; set; }
        public IndexLevel Pm25IndexLevel { get; set; }
        public DateTime? Pm25SourceDataDate { get; set; }
        public DateTime? O3CalcDate { get; set; }
        public IndexLevel O3IndexLevel { get; set; }
        public DateTime? O3SourceDataDate { get; set; }
        public bool StIndexStatus { get; set; }
        public string StIndexCrParam { get; set; }
    }
}
