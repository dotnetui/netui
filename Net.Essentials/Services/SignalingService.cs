using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials
{
    public class SignalingService : Singleton<SignalingService>
    {
        readonly Dictionary<Type, Action> delegates = new Dictionary<Type, Action>();

        public SignalingService() { }

        public void Subscribe<T>(Action action) where T : Signal
        {
            if (delegates.ContainsKey(typeof(T)))
                delegates[typeof(T)] += action;
            else
                delegates.Add(typeof(T), action);
        }

        public void Unsubscribe<T>(Action action) where T : Signal
        {
            if (delegates.ContainsKey(typeof(T)))
                delegates[typeof(T)] -= action;
        }
    }
}
