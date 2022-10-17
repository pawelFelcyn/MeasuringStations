using MeasuringStations.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations
{
    public class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private Station _station;
        
    }
}
