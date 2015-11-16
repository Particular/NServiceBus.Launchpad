//# namespace {{endpointName}} 
//# {
    using System;
    using NServiceBus;
    //# using Ignited.NServiceBus.Shared;

    public class MessageHandler : IHandleMessages<MessagePlaceholder>
    {
        public void Handle(MessagePlaceholder message)
        {
            Console.WriteLine(message.GetType() + " received.");
        }
    }

//# }