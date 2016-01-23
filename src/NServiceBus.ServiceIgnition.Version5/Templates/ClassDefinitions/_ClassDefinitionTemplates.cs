// This file is auto-generated, any changes you make to this file will be overwritten

using System.Collections.Generic;

public static class ClassDefinitionTemplates
{
    public static Dictionary<string, string> Dictionary = new Dictionary<string, string>()
    {
        { "EventPlaceholder", @"using NServiceBus;

namespace Ignited.NServiceBus.Shared
{
    public class EventPlaceholder : IEvent
    {
    }
}" },
        { "MessageHandler", @"namespace {{endpointName}} 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class MessageHandler : IHandleMessages<MessagePlaceholder>
    {
        public void Handle(MessagePlaceholder message)
        {
            Console.WriteLine(message.GetType() + "" received."");
        }
    }

}" },
        { "MessagePlaceholder", @"using NServiceBus;

namespace Ignited.NServiceBus.Shared
{
    public class MessagePlaceholder : IMessage
    {
    }
}" },
        { "Program", @"using NServiceBus;
using Ignited.NServiceBus.Shared;

namespace Ignited.NServiceBus.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var endpointConfiguration = new BusConfiguration();
            endpointConfiguration.EndpointName(""Ignited.NServiceBus.Console"");
            {{configurationDetails}}

#if DEBUG
        //Enable installers is not to be run in production environments. It is for development purposes only.
        endpointConfiguration.EnableInstallers();
#endif

            using (IBus bus = Bus.Create(endpointConfiguration))
            {
                {{busExampleCalls}}
            }
        }
    }
}
" },
        { "ProgramService", @"namespace {{endpointName}}
{

    using System;
    using System.ServiceProcess;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    /// <summary>
    /// http://docs.particular.net/nservicebus/hosting/windows-service
    /// </summary>
    [System.ComponentModel.DesignerCategory(""Code"")]
    public class ProgramService : ServiceBase
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
                    Console.WriteLine(""Press enter to exit"");
                    Console.ReadLine();

                    service.OnStop();

                    return;
                }

                Run(service);
            }
        }

        protected static BusConfiguration ConfigureBus()
        {
            var endpointConfiguration = new BusConfiguration();

            endpointConfiguration.EndpointName(""{{endpointName}}"");

            {{configurationDetails}}
#if DEBUG
        //Enable installers is not to be run in production environments. It is for development purposes only.
        endpointConfiguration.EnableInstallers();
#endif

            return endpointConfiguration;
        }

        protected override void OnStart(string[] args)
        {
            var endpointConfiguration = ConfigureBus();
            bus = Bus.Create(endpointConfiguration).Start();
            {{codeSubscriptions}}
        }

        protected override void OnStop()
        {
            if (bus != null)
            {
                bus.Dispose();
            }
        }
    }

}
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