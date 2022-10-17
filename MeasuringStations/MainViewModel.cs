using MeasuringStations.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
    }
}
