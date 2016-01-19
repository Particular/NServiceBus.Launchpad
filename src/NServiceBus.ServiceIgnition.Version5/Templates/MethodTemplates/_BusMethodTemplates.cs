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
        { "PersistenceMethods.InMemory", @"// This is not suitable for production use unless you don't care about losing subscriptions on restart
            busConfiguration.UsePersistence<InMemoryPersistence>();" },
        { "PersistenceMethods.Msmq", @"busConfiguration.UsePersistence<MsmqPersistence>();" },
        { "PersistenceMethods.NHibernate", @"throw new NotImplementedException(""You need to configure your connection string"");
            busConfiguration.UsePersistence<NHibernatePersistence>().ConnectionString(""Connection_string_goes_here"");" },
        { "PersistenceMethods.Raven", @"throw new NotImplementedException(""You need to configure your connection parameters"");
            var connectionParameters = new NServiceBus.RavenDB.ConnectionParameters
            {
                Url = ""set_your_raven_url"",
                // ApiKey = ""Set_Your_API_key"",
                // Credentials = null, // Set your credentials,
                // DatabaseName = ""Set_your_database_name""," },
        { "SerializerMethods.Json", @"busConfiguration.UseSerialization<JsonSerializer>();" },
        { "SerializerMethods.Xml", @"busConfiguration.UseSerialization<XmlSerializer>();" },
        { "SerializerMethods.Binary", @"busConfiguration.UseSerialization<BinarySerializer>();" },
        { "TransportMethods.Msmq", @"// Msmq is the default transport for NServiceBus
            // It has been included in the core library since the first version
            busConfiguration.UseTransport<MsmqTransport>();" },
        { "TransportMethods.Sql", @"busConfiguration.UseTransport<SqlServerTransport>()                .ConnectionString(() => { throw new NotImplementedException(""You need to configure your connection string"");" },
        { "TransportMethods.Rabbit", @"busConfiguration.UseTransport<RabbitMQTransport>()                .ConnectionString(() => { throw new NotImplementedException(""You need to configure your connection string"");" },
        { "TransportMethods.AzureServiceBus", @"busConfiguration.UseTransport<AzureServiceBusTransport>()                .ConnectionString(() => { throw new NotImplementedException(""You need to configure your connection string"");" },
    }; 
}