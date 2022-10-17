using MeasuringStations.Models;
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
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsNotBusy))]
        private bool _isBusy;
        [ObservableProperty]
        private StationDetails _station;
        [ObservableProperty]
        private string _selectedStationName;
        public ObservableCollection<Station> AllStations { get; }
        public bool IsNotBusy => !IsBusy;

        public MainViewModel()
        {
            AllStations = new();
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

                using var client = new HttpClient();
                var response = await client.GetAsync("https://api.gios.gov.pl/pjp-api/rest/aqindex/getIndex/" + selectedStation.Id);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("Operation failed.");
                }

                var json = await response.Content.ReadAsStringAsync();
                Station = JsonConvert.DeserializeObject<StationDetails>(json);
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

                using var client = new HttpClient();
                var response = await client.GetAsync("https://api.gios.gov.pl/pjp-api/rest/station/findAll");

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("Couldn't load stations.");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var stations = JsonConvert.DeserializeObject<IEnumerable<Station>>(json);

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
