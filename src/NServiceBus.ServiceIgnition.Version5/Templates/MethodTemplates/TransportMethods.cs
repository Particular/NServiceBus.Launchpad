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
            //{ Transport.AzureStorageQueue, AzureStorageQueue},
        };

        public static void Msmq(BusConfiguration busConfiguration)
        {
            // Msmq is the default transport for NServiceBus.
            // It has been included in the core library since the first version.
            busConfiguration.UseTransport<MsmqTransport>();
        }

        public static void Sql(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<SqlServerTransport>();
        }

        public static void Rabbit(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<RabbitMQTransport>();
        }

        public static void AzureServiceBus(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<AzureServiceBusTransport>();
        }

        public static void AzureStorageQueue(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<AzureStorageQueueTransport>();
        }
    }
}
