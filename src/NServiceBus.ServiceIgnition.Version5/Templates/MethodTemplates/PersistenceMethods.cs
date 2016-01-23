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

        public static void None(BusConfiguration endpointConfiguration)
        {
            // Having no persistence configured is only suitable for transports which support native publish-subscribe
            // http://docs.particular.net/nservicebus/messaging/publish-subscribe/#mechanics-native-based
        }

        public static void InMemory(BusConfiguration endpointConfiguration)
        {
            // This is not suitable for production use
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
        }

        public static void Msmq(BusConfiguration endpointConfiguration)
        {
            endpointConfiguration.UsePersistence<MsmqPersistence>();
        }

        public static void NHibernate(BusConfiguration endpointConfiguration)
        {
            //# throw new NotImplementedException("You need to configure your connection string");
            endpointConfiguration.UsePersistence<NHibernatePersistence>().ConnectionString("Connection_string_goes_here");
        }

        public static void Raven(BusConfiguration endpointConfiguration)
        {
            //# throw new NotImplementedException("You need to configure your connection parameters");
            var connectionParameters = new NServiceBus.RavenDB.ConnectionParameters
            {
                Url = "Set_your_raven_url",
                // ApiKey = "Set_Your_API_key",
                // Credentials = null, // Set your credentials,
                // DatabaseName = "Set_your_database_name",
            };

            endpointConfiguration
                .UsePersistence<RavenDBPersistence>().
                SetDefaultDocumentStore(connectionParameters);
        }
    }
}