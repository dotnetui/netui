using Net.Essentials.Services;

[assembly: Dependency(typeof(RandomService))]
namespace Net.Essentials.Services;

public class RandomService
{
    public string GenerateId()
    {
        return IdExtensions.GenerateId();
    }
}
