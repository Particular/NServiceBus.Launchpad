// This file is auto-generated, any changes you make to this file will be overwritten

using System.Collections.Generic;

public static class BusMethodTemplates
{
    public static Dictionary<string, string> Dictionary = new Dictionary<string, string>()
    {
        { "BusMethods.Send", @"bus.Send(new MessagePlaceholder());" },
        { "BusMethods.Publish", @"bus.Publish(new EventPlaceholder());" },
        { "BusMethods.Subscribe", @"bus.Subscribe<MessagePlaceholder>();" },
        { "PersistenceMethods.None", @"// Having no persistence configured is only suitable for transports which support native publish-subscribe
            // http://docs.particular.net/nservicebus/messaging/publish-subscribe/#mechanics-native-based" },
        { "PersistenceMethods.InMemory", @"// This is not suitable for production use
            endpointConfiguration.UsePersistence<InMemoryPersistence>();" },
        { "PersistenceMethods.Msmq", @"endpointConfiguration.UsePersistence<MsmqPersistence>();" },
        { "PersistenceMethods.NHibernate", @"throw new NotImplementedException(""You need to configure your connection string"");
            endpointConfiguration.UsePersistence<NHibernatePersistence>().ConnectionString(""Connection_string_goes_here"");" },
        { "PersistenceMethods.Raven", @"throw new NotImplementedException(""You need to configure your connection parameters"");
            var connectionParameters = new NServiceBus.RavenDB.ConnectionParameters
            {
                Url = ""Set_your_raven_url"",
                // ApiKey = ""Set_Your_API_key"",
                // Credentials = null, // Set your credentials,
                // DatabaseName = ""Set_your_database_name""," },
        { "SerializerMethods.Json", @"endpointConfiguration.UseSerialization<JsonSerializer>();" },
        { "SerializerMethods.Xml", @"endpointConfiguration.UseSerialization<XmlSerializer>();" },
        { "SerializerMethods.Binary", @"endpointConfiguration.UseSerialization<BinarySerializer>();" },
        { "TransportMethods.Msmq", @"// Msmq is the default transport for NServiceBus
            // It has been included in the core library since the first version
            endpointConfiguration.UseTransport<MsmqTransport>();" },
        { "TransportMethods.Sql", @"endpointConfiguration.UseTransport<SqlServerTransport>()                .ConnectionString(() => { throw new NotImplementedException(""You need to configure your connection string"");" },
        { "TransportMethods.Rabbit", @"endpointConfiguration.UseTransport<RabbitMQTransport>()                .ConnectionString(() => { throw new NotImplementedException(""You need to configure your connection string"");" },
        { "TransportMethods.AzureServiceBus", @"endpointConfiguration.UseTransport<AzureServiceBusTransport>()                .ConnectionString(() => { throw new NotImplementedException(""You need to configure your connection string"");" },
    }; 
}