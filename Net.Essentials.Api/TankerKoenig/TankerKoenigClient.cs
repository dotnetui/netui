using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials.TankerKoenig
{
    public class TankerKoenigClient
    {
        public enum Sort
        {
            Distance, //dist
            Price //price
        }

        public enum Type
        {
            All,
            E5,
            E10,
            Diesel,
        }

        public async Task<List<Station>> GetAsync(
            string apikey,
            double latitude,
            double longitude,
            double radius = 25,
            Sort sort = Sort.Distance,
            Type type = Type.All,
            bool safe = true)
        {
            try
            {
                var url = GetUrl(
                    apikey,
                    latitude,
                    longitude,
                    radius,
                    sort,
                    type);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Host", "creativecommons.tankerkoenig.de");
                var json = await client.GetStringAsync(url);
                var response = JsonConvert.DeserializeObject<ApiResponse>(json);
                return response?.Stations;
            }
            catch
            {
                if (!safe) throw;
            }
            return null;
        }

        public string GetUrl(
            string apikey,
            double latitude,
            double longitude,
            double radius = 25,
            Sort sort = Sort.Distance,
            Type type = Type.All)
        {
            var sortString = sort == Sort.Distance ? "dist" : "price";
            var typeString = type.ToString().ToLower();
            var url =
                "https://creativecommons.tankerkoenig.de/json/list.php?" +
                $"lat={latitude}&" +
                $"lng={longitude}&" +
                $"rad={radius}&" +
                $"sort={sortString}&" +
                $"type={typeString}&" +
                $"apikey={apikey}";
            return url;
        }

        class ApiResponse
        {
            public List<Station> Stations { get; set; }
        }
    }
}