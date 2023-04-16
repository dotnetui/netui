using Newtonsoft.Json;

namespace Net.Essentials.TankerKoenig
{
    public class Station
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Street { get; set; }
        public string Place { get; set; }
        [JsonProperty("lat")] public double Latitude { get; set; }
        [JsonProperty("lng")] public double Longitude { get; set; }
        [JsonProperty("dist")] public double Distance { get; set; }
        public decimal? Diesel { get; set; }
        public decimal? E5 { get; set; }
        public decimal? E10 { get; set; }
        public bool IsOpen { get; set; }
        public string HouseNumber { get; set; }
        public string PostCode { get; set; }
    }
}
