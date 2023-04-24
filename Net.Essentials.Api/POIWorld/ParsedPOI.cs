using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.POIWorld
{
    public class ParsedPOI : IParsedPOI
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string CountryCode { get; set; }
        public string Image { get; set; }
        public string Wikipedia { get; set; }
        public string WikidataId { get; set; }
        public string Continent { get; set; }
    }
}
