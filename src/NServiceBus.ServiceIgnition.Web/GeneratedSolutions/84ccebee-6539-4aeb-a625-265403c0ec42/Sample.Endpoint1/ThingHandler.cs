namespace Sample.Endpoint1 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class ThingHandler : IHandleMessages<Thing>
    {
        public void Handle(Thing message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}