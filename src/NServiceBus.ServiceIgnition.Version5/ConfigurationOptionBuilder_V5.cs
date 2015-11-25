namespace NServiceBus.ServiceIgnition
{
    public class ConfigurationOptionBuilder_V5 : AbstractVersionOptionBuilder
    {
        public override VersionConfigurationOptions GetConfigurationOptions()
        {
            var version = new VersionConfigurationOptions()
            {
                NServiceBusVersion = NServiceBusVersion.Five,
            };

            version.PersistenceSection =
                MakeConfigurationSection(
                    Persistence.InMemory,
                    Persistence.Msmq,
                    Persistence.NHibernate,
                    Persistence.RavenDB
                    //Persistence.AzureStorage
                    );

            version.SerializerSection =
                MakeConfigurationSection(
                    Serializer.Json,
                    Serializer.Xml,
                    Serializer.Binary
                    );

            version.TransportSection =
                MakeConfigurationSection(
                    Transport.Msmq,
                    //Transport.AzureServiceBus,
                    //Transport.RabbitMQ,
                    Transport.SqlServer
                    );

            return version;
        }
    }
}
