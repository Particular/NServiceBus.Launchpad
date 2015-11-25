// This file is auto-generated, any changes you make to this file will be overwritten

using System.Collections.Generic;

public static class BusMethodTemplates
{
    public static Dictionary<string, string> Dictionary = new Dictionary<string, string>()
    {
        { "BusMethods.Send", @"bus.Send(new MessagePlaceholder());" },
        { "BusMethods.Publish", @"bus.Publish(new EventPlaceholder());" },
        { "PersistenceMethods.None", @"// Having no persistence configured is only suitable for transports which support native publish-subscribe
            // http://docs.particular.net/nservicebus/messaging/publish-subscribe/#mechanics-native-based" },
        { "PersistenceMethods.InMemory", @"// This is not suitable for production use
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