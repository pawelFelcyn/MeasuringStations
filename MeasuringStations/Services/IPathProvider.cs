using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations.Services
{
    public interface IPathProvider
    {
        bool TryGetPath(out string path);
    }
}
