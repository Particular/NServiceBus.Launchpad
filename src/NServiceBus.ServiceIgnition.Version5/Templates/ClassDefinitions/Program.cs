using NServiceBus;
//# using Ignited.NServiceBus.Shared;

//# namespace Ignited.NServiceBus.Console
//# {
    public class Program
    {
        static void Main(string[] args)
        {
            var endpointConfiguration = new BusConfiguration();
            endpointConfiguration.EndpointName("Ignited.NServiceBus.Console");
            //# {{configurationDetails}}

#if DEBUG
        //Enable installers is not to be run in production environments. It is for development purposes only.
        endpointConfiguration.EnableInstallers();
#endif

            using (IBus bus = Bus.Create(endpointConfiguration))
            {
                //# {{busExampleCalls}}
            }
        }
    }
//# }
