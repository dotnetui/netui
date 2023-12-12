using System;
using System.Collections.Generic;

namespace Net.Essentials
{
    public class ContainerService
    {
        public static Func<ContainerService> GetInstanceOverrideFunc;
        public static Action<ContainerService> SetInstanceOverrideFunc;

        static ContainerService _defaultInstance;

        static ContainerService GetInstance()
        {
            if (GetInstanceOverrideFunc != null)
                return GetInstanceOverrideFunc();

            if (_defaultInstance == null)
                _defaultInstance = new ContainerService();

            return _defaultInstance;
        }

        static void SetInstance(ContainerService instance)
        {
            if (SetInstanceOverrideFunc != null)
                SetInstance(instance);
            else
                _defaultInstance = instance;
        }

        public static ContainerService Instance
        {
            get => GetInstance();
            set => SetInstance(value);
        }

        public Dictionary<Type, object> Children { get; private set; } =
            new Dictionary<Type, object>();

        public T Get<T>() 
        {
            return Children.TryGetValue(typeof(T), out var vm) ? (T)vm : default;
        }

        public object Get(Type type)
        {
            return Children.TryGetValue(type, out var vm) ? vm : default;
        }

        public void Set<T>(T vm) 
        {
            Set(typeof(T), vm);
        }

        public void Set(object vm)
        {
            Set(vm.GetType(), vm);
        }

        public void Set(Type type, object vm)
        {
            Children[type] = vm;
        }

        public T GetOrCreate<T>() where T : class, new()
        {
            return GetOrCreate(() => new T());
        }

        public T GetOrCreate<T>(Func<T> factory) where T : class, new()
        {
            return (T)GetOrCreate(typeof(T), () => (T)factory());
        }

        public object GetOrCreate(Type type)
        {
            return GetOrCreate(type, () => Activator.CreateInstance(type));
        }

        public object GetOrCreate(Type type, Func<object> factory)
        {
            var vm = Get(type);
            if (vm == default)
            {
                vm = factory();
                Set(vm);
            }
            return vm;
        }
    }
}