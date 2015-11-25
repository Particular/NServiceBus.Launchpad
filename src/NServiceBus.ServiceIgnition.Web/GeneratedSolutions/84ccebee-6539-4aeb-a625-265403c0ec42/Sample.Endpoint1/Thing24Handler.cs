namespace Sample.Endpoint1 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class Thing24Handler : IHandleMessages<Thing24>
    {
        public void Handle(Thing24 message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}