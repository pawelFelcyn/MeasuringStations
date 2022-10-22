using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations.Services
{
    internal class FIleDIalogPathProvider : IPathProvider
    {
        public bool TryGetPath(out string path)
        {
            var dialog = new SaveFileDialog();

            var result = dialog.ShowDialog();

            if (!result.HasValue || !result.Value)
            {
                path = default(string);
                return false;
            }

            path = dialog.FileName;
            return true;
        }
    }
}
