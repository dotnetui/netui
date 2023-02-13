[assembly: Dependency(typeof(RandomService))]
namespace Net.Essentials;

public class RandomService
{
    public string GenerateId()
    {
        return IdExtensions.GenerateId();
    }
}
