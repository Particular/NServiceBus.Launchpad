//# namespace {{endpointName}}
//# {
    using NServiceBus;
    using NServiceBus.Transports;
    using NServiceBus.Serializers;
    using NServiceBus.Persistence;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("{{endpointName}}");
        //# {{configurationDetails}}
#if DEBUG
        //Enable installers is not to be run in production environments. It is for development purposes only.
        busConfiguration.EnableInstallers();
#endif
    }
}

//# }