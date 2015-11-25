namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class rrrereerHandler : IHandleMessages<rrrereer>
    {
        public void Handle(rrrereer message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}