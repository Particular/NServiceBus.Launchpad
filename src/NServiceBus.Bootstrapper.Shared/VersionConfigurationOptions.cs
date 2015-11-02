namespace NServiceBus.Bootstrapper
{
    using System.Collections.Generic;

    public class VersionConfigurationOptions
    {
        public NServiceBusVersion NServiceBusVersion { get; set; }
        public List<ConfigurationItem<Transport>> AvailableTransports { get; set; }
        public List<ConfigurationItem<Persistence>> AvailablePersistence { get; set; }
        public List<ConfigurationItem<Serializer>> AvailableSerializers { get; set; }
    }
}