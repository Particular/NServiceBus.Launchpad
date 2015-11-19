// This file is auto-generated, any changes you make to this file will be overwritten

using System.Collections.Generic;

public static class BusMethodTemplates
{
    public static Dictionary<string, string> Dictionary = new Dictionary<string, string>()
    {
        { "PersistenceMethods.None", @"// no persistence, persistence is optional" },
        { "PersistenceMethods.InMemory", @"// This is not suitable for production use
            //Please choose a more suitable persistence unless you don't care if you lose subscriptions on restart
            busConfiguration.UsePersistence<InMemoryPersistence>();" },
        { "PersistenceMethods.Msmq", @"busConfiguration.UsePersistence<MsmqPersistence>();" },
        { "PersistenceMethods.NHibernate", @"busConfiguration.UsePersistence<NHibernatePersistence>();" },
        { "PersistenceMethods.Raven", @"busConfiguration.UsePersistence<RavenDBPersistence>();" },
        { "SerializerMethods.Json", @"busConfiguration.UseSerialization<JsonSerializer>();" },
        { "SerializerMethods.Xml", @"busConfiguration.UseSerialization<XmlSerializer>();" },
        { "SerializerMethods.Binary", @"busConfiguration.UseSerialization<BinarySerializer>();" },
        { "TransportMethods.Msmq", @"// Msmq is the default transport for NServiceBus.
            // It has been included in the core library since the first version.
            busConfiguration.UseTransport<MsmqTransport>();" },
        { "TransportMethods.Sql", @"busConfiguration.UseTransport<SqlServerTransport>();" },
        { "TransportMethods.Rabbit", @"busConfiguration.UseTransport<RabbitMQTransport>();" },
        { "TransportMethods.AzureServiceBus", @"busConfiguration.UseTransport<AzureServiceBusTransport>();" },
        { "TransportMethods.AzureStorageQueue", @"busConfiguration.UseTransport<AzureStorageQueueTransport>();" },
    }; 
}