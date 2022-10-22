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
        private readonly INotifier _notifier;

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
            IStationFileSaverFactory stationFileSaverFactory, INotifier notifier)
        {
            AllStations = new();
            _service = service;
            _pathProvider = pathProvider;
            _stationFileSaverFactory = stationFileSaverFactory;
            _notifier = notifier;
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
                    _notifier.Notify("Couldn't find given station.");
                    return;
                }

                Station = await _service.GetDetailsAsync(selectedStation.Id);
            }
            catch
            {
                NotifyFailure();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void NotifyFailure()
        {
            _notifier.Notify("Operaton failed.");
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

                var stations = await _service.GetAllAsync();

                AllStations.Clear();
                foreach (var station in stations)
                {
                    AllStations.Add(station);
                }
            }
            catch
            {
                _notifier.Notify("Couldn't load stations.");
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
                    _notifier.Notify("Station to save has not been loaded.");
                    return;
                }

                if (!_pathProvider.TryGetPath(out var path))
                {
                    return;
                }

                var extension = ExtensionOf(path);
                var saver = _stationFileSaverFactory.Create(extension);
                await saver.SaveAsync(Station, path);
                NotifySuccess();
            }
            catch (NotSupportedExtensionException)
            {
                _notifier.Notify("This extension is not supported.");
            }
            catch(Exception)
            {
                NotifyFailure();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void NotifySuccess()
        {
            _notifier.Notify("Operation succeed.");
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
