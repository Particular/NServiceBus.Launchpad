namespace Sample.Endpoint1 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class rgergergregrHandler : IHandleMessages<rgergergregr>
    {
        public void Handle(rgergergregr message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}