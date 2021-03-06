﻿using System;

namespace Sample.Infrastructure.Remoting.Contracts
{
    public interface IListener<TInterface, out TMessage> where TMessage : IRemoteMessage
    {
        void StartPolling();
        void AddHandler(Func<TMessage, bool> handler);
    }
}