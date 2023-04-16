using System.Threading.Tasks;
using System.Collections.Generic;

namespace Net.Essentials.Places
{
    public interface IPlaces
    {
        Task<List<Place>> SearchAsync(string query);
        Task<List<Place>> SearchAsync(string query, double centerLatitude, double centerLongitude);
    }
}