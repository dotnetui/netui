using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        public static Func<T> GetInstanceOverrideFunc;
        public static Action<T> SetInstanceOverrideFunc;

        static T GetInstance()
        {
            if (GetInstanceOverrideFunc != null)
                return GetInstanceOverrideFunc();

            return ContainerService.Instance.GetOrCreate<T>();
        }

        static void SetInstance(Singleton<T> instance)
        {
            if (SetInstanceOverrideFunc != null)
                SetInstance(instance);
            else
                ContainerService.Instance.Set(typeof(T), instance);
        }

        public static T Instance
        {
            get => GetInstance();
            set => SetInstance(value);
        }
    }

}
