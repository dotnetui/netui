namespace Net.Places;

public interface IPlaces
{
    Task<List<Place>> SearchAsync(string query);
    Task<List<Place>> SearchAsync(string query, double centerLatitude, double centerLongitude);
}