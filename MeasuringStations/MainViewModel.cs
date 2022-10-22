using MeasuringStations.Exceptions;
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
        private readonly IPathProvider _pathProvider;
        private readonly IStationFileSaverFactory _stationFileSaverFactory;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsNotBusy))]
        private bool _isBusy;
        [ObservableProperty]
        private StationDetails _station;
        [ObservableProperty]
        private string _selectedStationName;
        public ObservableCollection<Station> AllStations { get; }
        public bool IsNotBusy => !IsBusy;

        public MainViewModel(IStationService service, IPathProvider pathProvider, 
            IStationFileSaverFactory stationFileSaverFactory)
        {
            AllStations = new();
            _service = service;
            _pathProvider = pathProvider;
            _stationFileSaverFactory = stationFileSaverFactory;
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
        [ICommand]
        private async Task SaveToFileAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                if (Station is null)
                {
                    MessageBox.Show("Station to save has not been loaded.");
                    return;
                }

                if (!_pathProvider.TryGetPath(out var path))
                {
                    return;
                }

                var extension = ExtensionOf(path);
                var saver = _stationFileSaverFactory.Create(extension);
                await saver.SaveAsync(Station, path);
                MessageBox.Show("Operation succeed.");
            }
            catch (NotSupportedExtensionException)
            {
                MessageBox.Show("This extension is not supported.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Operation failed");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string ExtensionOf(string path)
        {
            var lastDot = path.LastIndexOf('.');

            if (lastDot == -1)
            {
                throw new ArgumentException(nameof(path));
            }

            return path.Substring(lastDot + 1, path.Length - lastDot - 1);
        }
    }
}
