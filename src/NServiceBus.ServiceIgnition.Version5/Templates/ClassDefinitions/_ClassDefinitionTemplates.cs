// This file is auto-generated, any changes you make to this file will be overwritten

using System.Collections.Generic;

public static class ClassDefinitionTemplates
{
    public static Dictionary<string, string> Dictionary = new Dictionary<string, string>()
    {
        { "EventPlaceholder", @"using NServiceBus;

//# namespace Ignited.NServiceBus.Shared
//# {
    public class EventPlaceholder : IEvent
    {
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
        { "Program", @"using NServiceBus;
//# using Ignited.NServiceBus.Shared;

//# namespace Ignited.NServiceBus.Console
//# {
    public class Program
    {
        static void Main(string[] args)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(""Ignited.NServiceBus.Console"");
            //# {{configurationDetails}}

    #if DEBUG
            //Enable installers is not to be run in production environments. It is for development purposes only.
            busConfiguration.EnableInstallers();
    #endif

            using (IBus bus = Bus.Create(busConfiguration))
            {
                //# {{busExampleCalls}}

                // TODO: Published events are not picked up, fix it
            }
        }
    }
//# }
" },
        { "ProgramService", @"//# namespace {{endpointName}}
//# {

    using System;
    using System.ServiceProcess;
    using NServiceBus;
    using NServiceBus.Transports;
    using NServiceBus.Serializers;
    using NServiceBus.Persistence;

    /// <summary>
    /// http://docs.particular.net/nservicebus/hosting/windows-service
    /// </summary>
    class ProgramService : ServiceBase
    {
        IBus bus;

        static void Main()
        {
            using (ProgramService service = new ProgramService())
            {
                if (Environment.UserInteractive)
                {
                    service.OnStart(null);

                    Console.WriteLine(""Bus created and configured"");
                    Console.WriteLine(""Press any key to exit"");
                    Console.ReadKey();

                    service.OnStop();

                    return;
                }

                Run(service);
            }
        }

        protected static BusConfiguration ConfigureBus()
        {
            var busConfiguration = new BusConfiguration();

            busConfiguration.EndpointName(""{{endpointName}}"");

            //# {{configurationDetails}}

#if DEBUG
            //Enable installers is not to be run in production environments. It is for development purposes only.
            busConfiguration.EnableInstallers();
#endif

            return busConfiguration;
        }

        protected override void OnStart(string[] args)
        {
            var busConfiguration = ConfigureBus();
            bus = Bus.Create(busConfiguration).Start();
        }

        protected override void OnStop()
        {
            if (bus != null)
            {
                bus.Dispose();
            }
        }
    }

//# }
" },
        { "ProvideErrorConfiguration", @"using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

public class ProvideErrorConfiguration : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
{
    public MessageForwardingInCaseOfFaultConfig GetConfiguration()
    {
        return new MessageForwardingInCaseOfFaultConfig
        {
            ErrorQueue = ""error""
        };
    }
}" },
    }; 
}