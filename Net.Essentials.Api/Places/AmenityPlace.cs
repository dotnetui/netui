using System.Collections.Generic;

namespace Net.Essentials.Places
{
    public class AmenityPlace
    {
        public string Id { get; set; }
        public string Amenity { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public List<AmenityPlace> Nodes { get; set; }
        public AmenityPlaceType Type { get; set; }
    }

    public enum AmenityPlaceType
    {
        Unknown,
        Node,
        Way
    }
}