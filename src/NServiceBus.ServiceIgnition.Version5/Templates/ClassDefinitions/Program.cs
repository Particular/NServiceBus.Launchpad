using NServiceBus;
//# using Ignited.NServiceBus.Shared;

//# namespace Ignited.NServiceBus.Console
//# {
    public class Program
    {
        static void Main(string[] args)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Ignited.NServiceBus.Console");
            //# {{configurationDetails}}

#if DEBUG
        //Enable installers is not to be run in production environments. It is for development purposes only.
        busConfiguration.EnableInstallers();
#endif

            using (IBus bus = Bus.Create(busConfiguration))
            {
                //# {{busExampleCalls}}
            }
        }
    }
//# }
