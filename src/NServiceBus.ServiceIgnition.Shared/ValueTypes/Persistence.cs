namespace NServiceBus.ServiceIgnition
{
    public enum Persistence
    {
        None,
        InMemory,
        NHibernate,
        RavenDB,
        AzureStorage,
        Msmq
    }
}