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
                    Console.WriteLine("Press any key to exit");
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

            busConfiguration.EndpointName("{{endpointName}}");

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
