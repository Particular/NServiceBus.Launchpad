namespace NServiceBus.Bootstrapper
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