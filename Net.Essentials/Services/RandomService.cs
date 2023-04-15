using Net.Essentials;

namespace Net.Essentials
{
    public class RandomService : Singleton<RandomService>
    {
        public string GenerateId()
        {
            return IdExtensions.GenerateId();
        }
    }
}