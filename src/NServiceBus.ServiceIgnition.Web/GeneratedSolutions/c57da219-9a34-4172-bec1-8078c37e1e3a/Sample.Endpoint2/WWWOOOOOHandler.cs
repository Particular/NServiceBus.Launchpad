namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class WWWOOOOOHandler : IHandleMessages<WWWOOOOO>
    {
        public void Handle(WWWOOOOO message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}