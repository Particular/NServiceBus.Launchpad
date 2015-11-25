namespace Sample.Endpoint1 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class ddfdfdfdfweervvvHandler : IHandleMessages<ddfdfdfdfweervvv>
    {
        public void Handle(ddfdfdfdfweervvv message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}