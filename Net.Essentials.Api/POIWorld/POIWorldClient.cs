using Newtonsoft.Json;
using RestSharp;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Net.Essentials.POIWorld
{
    public class POIWorldClient
    {
        public string Host { get; set; } = "https://poiworld.com";

        readonly string webKey;

        public POIWorldClient(string webKey)
        {
            this.webKey = webKey;
        }

        RestClient _client;
        RestClient Client
        {
            get
            {
                if (_client == null) _client = new RestClient();
                return _client;
            }
        }

        public async Task<List<ParsedPOI>> GetRegionAsync(double latMin, double lngMin, double latMax, double lngMax, int max, POIFilter? filter)
        {
            var url = $"{Host}/api/region?webkey={webKey}&" +
                $"latMin={latMin}&" +
                $"latMax={latMax}&" +
                $"lngMin={lngMin}&" +
                $"lngMax={lngMax}&" +
                $"max={max}";

            if (filter.HasValue) url += $"&filter={filter}";

            var request = new RestRequest(url);
            var response = await Client.ExecuteAsync(request);
            if (!response.IsSuccessful) return null;
            try
            {
                var json = JsonConvert.DeserializeObject<List<ParsedPOI>>(response.Content);
                return json;
            }
            catch
            {

            }
            return null;
        }

        public async Task<List<string>> FetchImagesAsync(string wikidataId)
        {
            var url = $"{Host}/api/imagesforwikidata?webkey={webKey}&wikidataId={wikidataId}";
            var request = new RestRequest(url);
            var response = await Client.ExecuteAsync(request);
            if (!response.IsSuccessful) return new List<string>();
            try
            {
                var json = JsonConvert.DeserializeObject<List<string>>(response.Content).Where(x => x != null && x.ToLower().StartsWith("http")).ToList();
                return json;
            }
            catch { }
            return new List<string>();
        }
    }
}
