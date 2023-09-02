using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class LocationDetails
    {
        [JsonProperty("formatted_address")] public string FormattedAddress { get; set; }
        [JsonProperty("latitude")] public double? Latitude { get; set; }
        [JsonProperty("longitude")] public double? Longitude { get; set; }
        [JsonProperty("city")] public string City { get; set; }
        [JsonProperty("state")] public string State { get; set; }
        [JsonProperty("neighborhood")] public string Neighborhood { get; set; }
        [JsonProperty("sub_locality")] public string SubLocality { get; set; }
        [JsonProperty("state_iso_code")] public string StateIsoCode { get; set; }
        [JsonProperty("country")] public string Country { get; set; }
        [JsonProperty("country_iso_code")] public string CountryIsoCode { get; set; }
    }
}
