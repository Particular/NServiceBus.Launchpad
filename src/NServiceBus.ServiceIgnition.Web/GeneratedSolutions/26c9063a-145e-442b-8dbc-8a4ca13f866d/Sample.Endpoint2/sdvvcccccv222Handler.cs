namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class sdvvcccccv222Handler : IHandleMessages<sdvvcccccv222>
    {
        public void Handle(sdvvcccccv222 message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}