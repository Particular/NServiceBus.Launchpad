using NServiceBus;
//# using Ignited.NServiceBus.Shared;

//# namespace Ignited.NServiceBus.Console
//# {
    public class Program
    {
        static void Main(string[] args)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("{{Ignited.NServiceBus.Console}}");
            //# {{configurationDetails}}

            var bus = Bus.CreateSendOnly(busConfiguration);

            //# {{busExampleCalls}}
        }
    }
//# }
