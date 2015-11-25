using System;
using System.Collections.Generic;

namespace NServiceBus.ServiceIgnition.Version5
{
    public static class BusMethods
    {
        public static Dictionary<BusMethod, Action<IBus>> MethodsDictionary = new Dictionary<BusMethod, Action<IBus>>()
        {
            { BusMethod.Send, Send },
            { BusMethod.Publish, Publish }
        };

        public static void Send(IBus bus)
        {
            bus.Send(new MessagePlaceholder());
        }

        public static void Publish(IBus bus)
        {
            bus.Publish(new EventPlaceholder());
        }
    }
}