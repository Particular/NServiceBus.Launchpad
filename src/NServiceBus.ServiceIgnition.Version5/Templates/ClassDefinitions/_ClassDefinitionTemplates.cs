// This file is auto-generated, any changes you make to this file will be overwritten

using System.Collections.Generic;

public static class ClassDefinitionTemplates
        {
            public static Dictionary<string, string> Dictionary = new Dictionary<string, string>()
            {
                                { "EndpointConfig", @"//# namespace {{endpointName}}
//# {
    using NServiceBus;
    using NServiceBus.Transports;
    using NServiceBus.Serializers;
    using NServiceBus.Persistence;

    public class EndpointConfig
    {
        public void Install()
        {
        // helo all
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(""{{endpointName}}"");
            //# {{configurationDetails}}
#if DEBUG
            //Enable installers is not to be run in production environments. It is for development purposes only.
            busConfiguration.EnableInstallers();
#endif
        }
    }

//# }" },
                { "MessageHandler", @"//# namespace {{endpointName}} 
//# {
    using System;
    using NServiceBus;
    //# using Ignited.NServiceBus.Shared;

    public class MessageHandler : IHandleMessages<MessagePlaceholder>
    {
        public void Handle(MessagePlaceholder message)
        {
            Console.WriteLine(message.GetType() + "" received."");
        }
    }

//# }" },
                { "MessagePlaceholder", @"using NServiceBus;

//# namespace Ignited.NServiceBus.Shared
//# {
    public class MessagePlaceholder : IMessage
    {
    }
//# }" },
            }; 
        }