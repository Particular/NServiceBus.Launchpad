namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class BigMessageHandler : IHandleMessages<BigMessage>
    {
        public void Handle(BigMessage message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}