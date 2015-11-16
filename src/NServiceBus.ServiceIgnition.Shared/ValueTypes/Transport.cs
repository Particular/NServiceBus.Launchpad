namespace NServiceBus.ServiceIgnition
{
    public enum Transport
    {
        Msmq,
        SqlServer,
        RabbitMQ,
        AzureServiceBus,
    }
    public enum InversionOfControl
    {
        Autofac,
        Ninject,
        CastleWindsor,
        StructureMap,
        Spring,
        Unity
    }
    public enum Logging
    {
        log4net,
        NLog,
    }
    public enum EndpointOptions
    {
        MsmqLoadBalancing,
        CommonLogging,
        HasSaga,
        SendOnly,
        NServiceBusHost,
        IsGateway,
    }
}