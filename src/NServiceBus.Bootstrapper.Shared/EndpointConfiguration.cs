namespace NServiceBus.Bootstrapper
{
    using System.Collections.Generic;

    public class EndpointConfiguration
    {
        public string EndpointName { get; set; }
        public NServiceBusVersion NServiceBusVersion { get; set; }
        public Transport Transport { get; set; }
        public Persistence Persistence { get; set; }
        public Serializer Serializer { get; set; }
        public List<MessageHandlerConfiguration> MessageHandlers { get; set; } 
    }

    public class MessageHandlerConfiguration
    {
        public string MessageTypeName { get; set; }
    }

    public class SolutionConfiguration
    {
        public List<EndpointConfiguration> EndpointConfigurations { get; set; } 
    }
}