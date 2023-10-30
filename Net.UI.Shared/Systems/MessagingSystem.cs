﻿using System;

#if XAMARIN
using Xamarin.Forms;
#endif

namespace Net.Essentials.Systems
{
    internal class MessagingSystem
    {
        MessagingSystem() { }
        public static MessagingSystem Instance { get; private set; } = new MessagingSystem();

        public void Subscribe(object subscriber, object message, Action callback)
        {
            MessagingCenter.Subscribe<MessagingSystem>(subscriber, message.ToString(), (s) => callback?.Invoke());
        }

        public void Subscribe<T>(object subscriber, object message, Action<T> callback)
        {
            MessagingCenter.Subscribe<MessagingSystem, T>(subscriber, message.ToString(), (s, t) => callback?.Invoke(t));
        }

        public void Unsubscribe(object subscriber, object message)
        {
            MessagingCenter.Unsubscribe<MessagingSystem>(subscriber, message.ToString());
        }

        public void Send(object message)
        {
            MessagingCenter.Send(this, message.ToString());
        }

        public void Send<T>(object message, T t)
        {
            MessagingCenter.Send(this, message.ToString(), t);
        }
    }
}