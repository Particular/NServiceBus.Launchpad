namespace NServiceBus.ServiceIgnition.Version5
{
    using System;
    using System.Collections.Generic;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.Legacy;

    public static class PersistenceMethods
    {
        public static Dictionary<Persistence, Action<BusConfiguration>> MethodsDictionary = new Dictionary<Persistence, Action<BusConfiguration>>()
        {
            { Persistence.None, None},
            { Persistence.InMemory, InMemory},
            { Persistence.Msmq, Msmq },
            { Persistence.NHibernate, NHibernate },
            { Persistence.RavenDB, Raven },
        };

        public static void None(BusConfiguration busConfiguration)
        {
            // Having no persistence configured is only suitable for transports which support native publish-subscribe
            // http://docs.particular.net/nservicebus/messaging/publish-subscribe/#mechanics-native-based
        }

        public static void InMemory(BusConfiguration busConfiguration)
        {
            // This is not suitable for production use unless you don't care about losing subscriptions on restart
            busConfiguration.UsePersistence<InMemoryPersistence>();
        }

        public static void Msmq(BusConfiguration busConfiguration)
        {
            busConfiguration.UsePersistence<MsmqPersistence>();
        }

        public static void NHibernate(BusConfiguration busConfiguration)
        {
            //# throw new NotImplementedException("You need to configure your connection string");
            busConfiguration.UsePersistence<NHibernatePersistence>().ConnectionString("Connection_string_goes_here");
        }

        public static void Raven(BusConfiguration busConfiguration)
        {
            busConfiguration.UsePersistence<RavenDBPersistence>();
        }
    }
}