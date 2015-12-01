namespace NServiceBus.ServiceIgnition.Version5
{
    using System;
    using System.Collections.Generic;

    public static class TransportMethods
    {
        public static Dictionary<Transport, Action<BusConfiguration>> MethodsDictionary = new Dictionary<Transport, Action<BusConfiguration>>()
        {
            { Transport.Msmq, Msmq },
            { Transport.SqlServer, Sql},
            { Transport.RabbitMQ, Rabbit},
            { Transport.AzureServiceBus, AzureServiceBus},
        };

        public static void Msmq(BusConfiguration busConfiguration)
        {
            // Msmq is the default transport for NServiceBus
            // It has been included in the core library since the first version
            busConfiguration.UseTransport<MsmqTransport>();
        }

        public static void Sql(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString(() => { throw new NotImplementedException("You need to configure your connection string"); });
        }

        public static void Rabbit(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<RabbitMQTransport>()
                .ConnectionString(() => { throw new NotImplementedException("You need to configure your connection string"); });
        }

        public static void AzureServiceBus(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<AzureServiceBusTransport>()
                .ConnectionString(() => { throw new NotImplementedException("You need to configure your connection string"); });
        }
    }
}
