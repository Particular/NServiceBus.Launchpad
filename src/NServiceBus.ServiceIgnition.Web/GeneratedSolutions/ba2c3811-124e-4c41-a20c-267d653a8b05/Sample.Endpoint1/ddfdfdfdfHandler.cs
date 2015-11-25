namespace Sample.Endpoint1 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class ddfdfdfdfHandler : IHandleMessages<ddfdfdfdf>
    {
        public void Handle(ddfdfdfdf message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}