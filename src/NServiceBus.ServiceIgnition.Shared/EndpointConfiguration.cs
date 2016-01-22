namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;

    public class EndpointConfiguration
    {
        public EndpointConfiguration()
        {
            ProjectGuid = Guid.NewGuid();
            MessageHandlers = new List<MessageHandlerConfiguration>();
        }

        public string EndpointName { get; set; }
        public Persistence Persistence { get; set; }
        public Serializer Serializer { get; set; }
        public NServiceBusVersion NServiceBusVersion { get; set; }
        public Transport Transport { get; set; }
        public List<MessageHandlerConfiguration> MessageHandlers { get; set; } 

        public Guid ProjectGuid { get; set; }
        public bool InCodeSubscriptions { get; set; }
    }

    public class MessageHandlerConfiguration
    {
        public string MessageTypeName { get; set; }
        public bool IsEvent { get; set; }
    }

    public class SolutionConfiguration
    {
        public NServiceBusVersion NServiceBusVersion { get; set; }
        public Serializer Serializer { get; set; }
        public Transport Transport { get; set; }
        public Persistence Persistence { get; set; }
        public List<EndpointConfiguration> EndpointConfigurations { get; set; } 
        public bool InCodeSubscriptions { get; set; }
    }
}