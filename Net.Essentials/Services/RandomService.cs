using Net.Services;

[assembly: Dependency(typeof(RandomService))]
namespace Net.Services;

public class RandomService
{
    public string GenerateId()
    {
        return IdExtensions.GenerateId();
    }
}
