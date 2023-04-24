using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.POIWorld
{
    public interface IParsedPOI : IRecord
    {
        string Name { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        string CountryCode { get; set; }
        string Image { get; set; }
        string Wikipedia { get; set; }
        string WikidataId { get; set; }
        string Continent { get; set; }
    }
}
