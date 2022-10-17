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
        private bool _isBusy;
        [ObservableProperty]
        private StationDetails _station;
        [ObservableProperty]
        private int _id = 291;
        public ObservableCollection<Station> AllStations { get; }

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
                using var client = new HttpClient();
                var response = await client.GetAsync("https://api.gios.gov.pl/pjp-api/rest/aqindex/getIndex/" + Id);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Station = JsonConvert.DeserializeObject<StationDetails>(json);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetAllStationsAsync()
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
