namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class SomeMessageHandler : IHandleMessages<SomeMessage>
    {
        public void Handle(SomeMessage message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}