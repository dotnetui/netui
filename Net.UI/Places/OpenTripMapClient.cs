using Newtonsoft.Json;
using RestSharp;

namespace Net.Places;

public class OpenTripMapClient
{
    public const string BaseUrl = "https://api.opentripmap.com/0.1/en/places/";
    public string ApiKey { get; set; }
    readonly RestClient client;

    public OpenTripMapClient(string apiKey)
    {
        client = new RestClient(BaseUrl);
        this.ApiKey = apiKey;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="lon"></param>
    /// <param name="radius">in meters</param>
    /// <param name="rate"></param>
    /// <returns></returns>
    public async Task<OpenTripMapSimpleFeature[]> GetFeaturesAsync(
        double radius,
        double lat,
        double lon,
        string name = null,
        string kinds = null,
        string rate = null,
        int? limit = null)
    {
        var request = new RestRequest("radius", Method.Get);
        request.AddQueryParameter("apikey", ApiKey);
        request.AddQueryParameter("radius", radius.ToString());
        request.AddQueryParameter("lat", lat.ToString());
        request.AddQueryParameter("lon", lon.ToString());
        if (name != null) request.AddQueryParameter("name", name);
        if (kinds != null) request.AddQueryParameter("kinds", kinds);
        if (rate != null) request.AddQueryParameter("rate", rate);
        if (limit != null) request.AddQueryParameter("limit", limit.ToString());
        request.AddQueryParameter("format", "json");
        var response = await client.ExecuteAsync(request);
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        var results = JsonConvert.DeserializeObject<SimpleFeature[]>(response.Content);
        return Convert(results);
    }

    public async Task<OpenTripMapSimpleFeature[]> GetFeaturesAsync(
        double lat_min,
        double lat_max,
        double lon_min,
        double lon_max,
        string name = null,
        string kinds = null,
        string rate = null,
        int? limit = null)
    {
        var request = new RestRequest("bbox", Method.Get);
        request.AddQueryParameter("apikey", ApiKey);
        request.AddQueryParameter("lon_min", lon_min.ToString());
        request.AddQueryParameter("lon_max", lon_max.ToString());
        request.AddQueryParameter("lat_min", lat_min.ToString());
        request.AddQueryParameter("lat_max", lat_max.ToString());
        if (name != null) request.AddQueryParameter("name", name);
        if (kinds != null) request.AddQueryParameter("kinds", kinds);
        if (rate != null) request.AddQueryParameter("rate", rate);
        if (limit != null) request.AddQueryParameter("limit", limit.ToString());
        request.AddQueryParameter("format", "json");
        var response = await client.ExecuteAsync(request);
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        var results = JsonConvert.DeserializeObject<SimpleFeature[]>(response.Content);
        return Convert(results);
    }

    public enum Props
    {
        Base,
        Address
    }

    public async Task<OpenTripMapSimpleFeature[]> GetFeaturesAsync(
        double radius,
        double lat,
        double lon,
        string name,
        Props props = Props.Base,
        string kinds = null,
        string rate = null,
        int? limit = null)
    {
        var request = new RestRequest("bbox", Method.Get);
        request.AddQueryParameter("apikey", ApiKey);
        request.AddQueryParameter("name", name);
        request.AddQueryParameter("radius", radius.ToString());
        request.AddQueryParameter("lon", lon.ToString());
        request.AddQueryParameter("lat", lat.ToString());
        request.AddQueryParameter("props", props.ToString().ToLower());
        if (kinds != null) request.AddQueryParameter("kinds", kinds);
        if (rate != null) request.AddQueryParameter("rate", rate);
        if (limit != null) request.AddQueryParameter("limit", limit.ToString());
        request.AddQueryParameter("format", "json");
        var response = await client.ExecuteAsync(request);
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        var results = JsonConvert.DeserializeObject<SimpleFeature[]>(response.Content);
        return Convert(results);
    }

    OpenTripMapSimpleFeature[] Convert(SimpleFeature[] results)
    {
        return results.Select(x => new OpenTripMapSimpleFeature
        {
            Distance = x.dist,
            Kinds = x.kinds?.Split(','),
            Latitude = x.point.lat,
            Longitude = x.point.lon,
            Name = x.name,
            OSM = x.osm,
            Rate = x.rate,
            Wikidata = x.wikidata,
            XId = x.xid
        }).ToArray();
    }

    public async Task<OpenTripMapGeoName> GetGeoNameAsync(string query)
    {
        var request = new RestRequest("geoname", Method.Get);
        request.AddQueryParameter("apikey", ApiKey);
        request.AddQueryParameter("name", query);
        var response = await client.ExecuteAsync(request);
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        return JsonConvert.DeserializeObject<OpenTripMapGeoName>(response.Content);
    }

    public async Task<OpenTripMapPlace> GetPlaceAsync(string xid)
    {
        var request = new RestRequest($"xid/{xid}", Method.Get);
        request.AddQueryParameter("apikey", ApiKey);
        var response = await client.ExecuteAsync(request);
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        var result = JsonConvert.DeserializeObject<X>(response.Content);
        return new OpenTripMapPlace
        {
            Address = result.address,
            Image = result.image,
            Latitude = result.point.lat,
            Longitude = result.point.lon,
            Name = result.name,
            OSM = result.osm,
            Preview = result.preview,
            Rate = result.rate,
            Wikipedia = result.wikidata,
            Kinds = result.kinds?.Split(','),
            OTM = result.otm,
            Wikidata = result.wikidata,
            WikipediaExtracts = result.wikipedia_extracts,
            XId = result.xid
        };
    }

    class SimpleFeature
    {
        public string xid { get; set; }
        public string name { get; set; }
        public double dist { get; set; }
        public string rate { get; set; }
        public string wikidata { get; set; }
        public string kinds { get; set; }
        public Point point { get; set; }
        public string osm { get; set; }
    }

    class X
    {
        public string xid { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
        public string rate { get; set; }
        public string osm { get; set; }
        public string wikidata { get; set; }
        public string kinds { get; set; }
        public string otm { get; set; }
        public string wikipedia { get; set; }
        public string image { get; set; }
        public Preview preview { get; set; }
        public WikipediaExtracts wikipedia_extracts { get; set; }
        public Point point { get; set; }
    }

    public class Preview
    {
        public string Source { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class WikipediaExtracts
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Road { get; set; }
        public string County { get; set; }
        public string Suburb { get; set; }
        public string Country { get; set; }
        [JsonProperty("postcode")] public string PostCode { get; set; }
        [JsonProperty("country_code")] public string CountryCode { get; set; }
    }

    class Point
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}