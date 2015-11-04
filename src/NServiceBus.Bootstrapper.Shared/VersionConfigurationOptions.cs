namespace NServiceBus.Bootstrapper
{
    public class VersionConfigurationOptions
    {
        public NServiceBusVersion NServiceBusVersion { get; set; }
        public ConfigurationSection<Transport> TransportSection { get; set; }
        public ConfigurationSection<Persistence> PersistenceSection { get; set; }
        public ConfigurationSection<Serializer> SerializerSection { get; set; }
    }
}