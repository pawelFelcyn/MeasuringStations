using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations.Services
{
    public interface IStationFileSaverFactory
    {
        IStationFileSaver Create(string extension);
    }
}
