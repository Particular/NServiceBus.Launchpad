namespace Sample.Endpoint2
{
    using NServiceBus;
    using NServiceBus.Transports;
    using NServiceBus.Serializers;
    using NServiceBus.Persistence;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Sample.Endpoint2");
            // Msmq is the default transport for NServiceBus.
            // It has been included in the core library since the first version.
            busConfiguration.UseTransport<MsmqTransport>();
            busConfiguration.UseSerialization<XmlSerializer>();
            // no persistence, persistence is optional

#if DEBUG
            //Enable installers is not to be run in production environments. It is for development purposes only.
            busConfiguration.EnableInstallers();
#endif
        }
    }

}