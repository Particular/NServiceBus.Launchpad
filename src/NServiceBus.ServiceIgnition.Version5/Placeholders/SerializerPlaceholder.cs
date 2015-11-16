namespace NServiceBus.ServiceIgnition
{
    using System;
    using NServiceBus.Serialization;

    public class SerializerPlaceholder : SerializationDefinition {
        protected override Type ProvidedByFeature()
        {
            throw new NotImplementedException();
        }
    }
}