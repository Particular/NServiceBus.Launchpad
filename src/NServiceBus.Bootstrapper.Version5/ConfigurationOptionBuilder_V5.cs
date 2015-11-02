namespace NServiceBus.Bootstrapper
{
    public class ConfigurationOptionBuilder_V5 : AbstractVersionOptionBuilder
    {
        public override VersionConfigurationOptions GetConfigurationOptions()
        {
            var version = new VersionConfigurationOptions()
            {
                NServiceBusVersion = NServiceBusVersion.Five,
            };

            version.AvailablePersistence =
                MakeConfigurationList(
                    Persistence.None,
                    Persistence.Msmq,
                    Persistence.NHibernate,
                    Persistence.RavenDB,
                    Persistence.AzureStorage);

            version.AvailableSerializers =
                MakeConfigurationList(
                    Serializer.Json,
                    Serializer.Xml,
                    Serializer.Binary);

            version.AvailableTransports =
                MakeConfigurationList(
                    Transport.AzureServiceBus,
                    Transport.Msmq,
                    Transport.RabbitMQ,
                    Transport.SqlServer);

            return version;
        }
    }
}
