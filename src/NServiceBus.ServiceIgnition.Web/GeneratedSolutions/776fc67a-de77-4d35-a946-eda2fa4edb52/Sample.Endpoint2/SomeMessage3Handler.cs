namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class SomeMessage3Handler : IHandleMessages<SomeMessage3>
    {
        public void Handle(SomeMessage3 message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}