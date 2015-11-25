namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class Thing2Handler : IHandleMessages<Thing2>
    {
        public void Handle(Thing2 message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}