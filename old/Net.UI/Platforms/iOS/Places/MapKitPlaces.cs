﻿using Contacts;
using CoreLocation;
using MapKit;

namespace Net.Essentials.Places;

public partial class MapKitPlaces : IPlaces
{
    public double DeltaLatitude { get; set; } = 0.25;
    public double DeltaLongitude { get; set; } = 0.25;

    public async Task<List<Place>> SearchAsync(string query)
    {
        return
            (await SearchAsync(query, false, 0, 0))
            .Select(x => MKToPlace(x))
            .ToList();
    }

    public async Task<List<Place>> SearchAsync(string query, double centerLatitude, double centerLongitude)
    {
        return
            (await SearchAsync(query, true, centerLatitude, centerLongitude, DeltaLatitude, DeltaLongitude))
            .Select(x => MKToPlace(x))
            .ToList();
    }

    public async Task<IEnumerable<MKMapItem>> SearchAsync(
        string query,
        bool aroundRegion,
        double centerLatitude,
        double centerLongitude,
        double deltaLatitude = 0.25,
        double deltaLongitude = 0.25)
    {
        var request = new MKLocalSearchRequest();
        request.NaturalLanguageQuery = query;

        if (aroundRegion)
        {
            var center = new CLLocationCoordinate2D(centerLatitude, centerLongitude);
            var span = new MKCoordinateSpan(deltaLatitude, deltaLongitude);
            request.Region = new MKCoordinateRegion(center, span);
        }

        var search = new MKLocalSearch(request);
        var response = await search.StartAsync();

        if (response == null) return null;
        if (response.MapItems == null) return null;

        return response.MapItems;
    }

    public static Place MKToPlace(MKMapItem item)
    {
        return new Place
        {
            //IsCurrentLocation = item.IsCurrentLocation,
            //PhoneNumber = item.PhoneNumber,
            //Street = item.Placemark?.PostalAddress?.Street,
            //City = item.Placemark?.PostalAddress?.City,
            //State = item.Placemark?.PostalAddress?.State,
            //PostalCode = item.Placemark?.PostalAddress?.PostalCode,
            //Country = item.Placemark?.PostalAddress?.Country,
            //Url = item.Url?.AbsoluteString,
            Name = item.Name,
            Latitude = item.Placemark?.Location?.Coordinate.Latitude ?? 0,
            Longitude = item.Placemark?.Location?.Coordinate.Longitude ?? 0,
            HasCoordinates = item.Placemark?.Location?.Coordinate != null,
            Address = GetAddress(item.Placemark?.PostalAddress),
            Tag = item
        };
    }

    static CNPostalAddressFormatter Formatter = new CNPostalAddressFormatter();

    internal static string GetAddress(CNPostalAddress address)
    {
        if (address == null) return null;
        return Formatter.GetStringFromPostalAddress(address);
    }
}