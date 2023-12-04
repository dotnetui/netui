using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials
{
    public static class Boot
    {
        public static readonly Type[] DependencyTypes = new Type[]
        {
            typeof(BenchmarkService),
            typeof(ContainerService),
            typeof(RandomService),
            typeof(ResourceService),
            typeof(SignalingService)
        };
    }
}
