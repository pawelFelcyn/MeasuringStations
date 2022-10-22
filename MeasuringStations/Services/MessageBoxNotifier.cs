using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MeasuringStations.Services
{
    internal class MessageBoxNotifier : INotifier
    {
        public void Notify(string notification)
        {
            MessageBox.Show(notification);
        }
    }
}
