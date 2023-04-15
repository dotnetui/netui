using System;

namespace Net.Essentials
{
    public static class IdExtensions
    {
        public static string GenerateId()
        {
            return $"{DateTime.UtcNow.Ticks}_{GuidId()}";
        }

        static string GuidId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}