namespace Sample.Endpoint2 
{
    using System;
    using NServiceBus;
    using Ignited.NServiceBus.Shared;

    public class WWWOOOOOe3wwHandler : IHandleMessages<WWWOOOOOe3ww>
    {
        public void Handle(WWWOOOOOe3ww message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

}