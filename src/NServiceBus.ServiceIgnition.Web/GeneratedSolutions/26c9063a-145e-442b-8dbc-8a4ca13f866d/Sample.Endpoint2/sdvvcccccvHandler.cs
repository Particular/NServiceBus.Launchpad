namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class sdvvcccccvHandler : IHandleMessages<sdvvcccccv>
    {
        public void Handle(sdvvcccccv message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}