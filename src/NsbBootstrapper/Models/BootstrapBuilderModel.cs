
using System;
using System.Collections.Generic;

namespace NsbBootstrapper.Models
{
    public enum NServiceBusVersion
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
    }
    public enum Transport
    {
        Msmq,
        SqlServer,
        RabbitMQ,
        AzureServiceBus,
    }
    public enum Persistence
    {
        None,
        InMemory,
        NHibernate,
        RavenDB,
        AzureStorage,
        Msmq
    }
    public enum Serializer
    {
        Xml,
        Json,
        Binary,
    }

    public class ConfigurationItem<T> where T : struct, IConvertible
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public List<string> RelevantLinks { get; set; }
    }

    public class VersionBuilderModel
    {
        public NServiceBusVersion NServiceBusVersion { get; set; }
        public List<ConfigurationItem<Transport>> AvailableTransports { get; set; }
        public List<ConfigurationItem<Persistence>> AvailablePersistence { get; set; }
        public List<ConfigurationItem<Serializer>> AvailableSerializers { get; set; }
    }

    public class BootstrapBuilderModel
    {
        public List<VersionBuilderModel> AvailableVersions { get; set; }
    }

    public class BootstrapModel
    {
        public NServiceBusVersion NServiceBusVersion { get; set; }
        public Transport Transport { get; set; }
        public Persistence Persistence { get; set; }
        public Serializer Serializer { get; set; }
    }

    public class ConfigurationResultModel
    {
        public string NuGetInstallers { get; set; }
        public string ConfigurationLogic { get; set; }
    }
}
