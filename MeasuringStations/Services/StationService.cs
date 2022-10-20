using MeasuringStations.Exceptions;
using MeasuringStations.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations.Services
{
    internal class StationService : IStationService
    {
        public async Task<IEnumerable<Station>> GetAllAsync()
        {
            var response = await HttpGetAsync("https://api.gios.gov.pl/pjp-api/rest/station/findAll");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new CouldntGetStationsException();
            }

            return await JsonContentToType<IEnumerable<Station>>(response.Content);
        }

        private async Task<HttpResponseMessage> HttpGetAsync(string uri)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            using var client = new HttpClient();
            return await client.SendAsync(message);
        }

        private async Task<T> JsonContentToType<T>(HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<StationDetails> GetDetailsAsync(int id)
        {
            var response = await HttpGetAsync("https://api.gios.gov.pl/pjp-api/rest/aqindex/getIndex/" + id);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new CouldntGetStationDetailsException();
            }

            return await JsonContentToType<StationDetails>(response.Content);
        }
    }
}
