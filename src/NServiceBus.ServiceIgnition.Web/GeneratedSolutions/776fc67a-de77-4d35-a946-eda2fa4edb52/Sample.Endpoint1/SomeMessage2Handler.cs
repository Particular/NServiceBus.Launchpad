namespace Sample.Endpoint1 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class SomeMessage2Handler : IHandleMessages<SomeMessage2>
    {
        public void Handle(SomeMessage2 message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}