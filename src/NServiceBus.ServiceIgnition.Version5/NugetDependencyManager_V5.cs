namespace NServiceBus.ServiceIgnition
{
    public class NugetDependencyManager_V5 : AbstractNuGetDependencyMapper
    {
        public NugetDependencyManager_V5()
        {
            AddDependency(NServiceBusVersion.Five, name: "NServiceBus", version: "5.2.6");

            //AddDependency(EndpointOptions.MsmqLoadBalancing, name: "NServiceBus.Distributor.MSMQ", version: "5.0.3");
            //AddDependency(EndpointOptions.CommonLogging, name: "NServiceBus.CommonLogging", version: "2.1.0");
            //AddDependency(EndpointOptions.IsGateway, name: "NServiceBus.Gateway", version: "1.0.1");
            //AddDependency(EndpointOptions.NServiceBusHost, name: "NServiceBus.Host", version: "6.0.0");

            AddDependency(Persistence.NHibernate, name: "NServiceBus.NHibernate", version: "6.2.2");
            AddDependency(Persistence.RavenDB, name: "NServiceBus.RavenDB", version: "3.0.1");
            AddDependency(Persistence.AzureStorage, name: "NServiceBus.Azure.Transports.WindowsAzureStorageQueues", version: "6.2.1");

            AddDependency(Logging.log4net, name: "NServiceBus.Log4Net", version: "1.0.0");
            AddDependency(Logging.NLog, name: "NServiceBus.NLog", version: "1.1.0");

            AddDependency(Transport.RabbitMQ, name: "NServiceBus.RabbitMQ", version: "3.0.0");
            AddDependency(Transport.SqlServer, name: "NServiceBus.SqlServer", version: "2.1.3");
            AddDependency(Transport.AzureServiceBus, name: "NServiceBus.Azure", version: "6.2.2");
            AddDependency(Transport.AzureServiceBus, name: "NServiceBus.Azure.Transports.WindowsAzureServiceBus", version: "6.3.2");

            AddDependency(InversionOfControl.Autofac, name: "NServiceBus.Autofac", version: "5.0.0");
            AddDependency(InversionOfControl.CastleWindsor, name: "NServiceBus.CastleWindsor", version: "5.0.0");
            AddDependency(InversionOfControl.Ninject, name: "NServiceBus.Ninject", version: "5.1.0");
            AddDependency(InversionOfControl.Spring, name: "NServiceBus.Spring", version: "6.0.0");
            AddDependency(InversionOfControl.StructureMap, name: "NServiceBus.StructureMap", version: "5.0.1");
            AddDependency(InversionOfControl.Unity, name: "NServiceBus.Unity", version: "6.2.0");
        }
    }
}