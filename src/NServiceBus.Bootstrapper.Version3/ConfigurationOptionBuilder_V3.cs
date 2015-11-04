namespace NServiceBus.Bootstrapper
{
    public class ConfigurationOptionBuilder_V3 : AbstractVersionOptionBuilder
    {
        public override VersionConfigurationOptions GetConfigurationOptions()
        {
            var version = new VersionConfigurationOptions()
            {
                NServiceBusVersion = NServiceBusVersion.Three,
            };

            version.PersistenceSection =
                MakeConfigurationSection(
                    Persistence.None,
                    Persistence.Msmq,
                    Persistence.NHibernate,
                    Persistence.RavenDB);

            version.SerializerSection =
                MakeConfigurationSection(
                    Serializer.Json,
                    Serializer.Xml,
                    Serializer.Binary);

            version.TransportSection =
                MakeConfigurationSection(
                    Transport.AzureServiceBus,
                    Transport.Msmq,
                    Transport.RabbitMQ,
                    Transport.SqlServer);

            return version;
        }
    }
}