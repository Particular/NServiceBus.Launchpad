//# namespace {{endpointName}}
//# {

    using System;
    using System.ServiceProcess;
    using NServiceBus;
    //# using Ignited.NServiceBus.Shared;

    /// <summary>
    /// http://docs.particular.net/nservicebus/hosting/windows-service
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
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

                    Console.WriteLine("Bus created and configured");
                    Console.WriteLine("Press enter to exit");
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

            endpointConfiguration.EndpointName("{{endpointName}}");

            //# {{configurationDetails}}
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
            //# {{codeSubscriptions}}
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
