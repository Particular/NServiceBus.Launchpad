namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class rgergerHandler : IHandleMessages<rgerger>
    {
        public void Handle(rgerger message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}