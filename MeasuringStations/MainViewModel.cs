using MeasuringStations.Models;
using MeasuringStations.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MeasuringStations
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IStationService _service;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsNotBusy))]
        private bool _isBusy;
        [ObservableProperty]
        private StationDetails _station;
        [ObservableProperty]
        private string _selectedStationName;
        public ObservableCollection<Station> AllStations { get; }
        public bool IsNotBusy => !IsBusy;

        public MainViewModel(IStationService service)
        {
            AllStations = new();
            _service = service;
        }

        [ICommand]
        private async Task GetStationAsync() 
        { 
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                var selectedStation = AllStations.FirstOrDefault(s => s.StationName == SelectedStationName?.Trim());
                if (selectedStation is null)
                {
                    MessageBox.Show("Couldn't find given station.");
                    return;
                }

                Station = await _service.GetDetailsAsync(selectedStation.Id);
            }
            catch (Exception)
            {
                MessageBox.Show("Operation failed.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ICommand]
        private async Task GetAllStationsAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                AllStations.Clear();

                var stations = await _service.GetAllAsync();

                foreach (var station in stations)
                {
                    AllStations.Add(station);
                }
            }
            catch
            {
                MessageBox.Show("Couldn't load stations.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
