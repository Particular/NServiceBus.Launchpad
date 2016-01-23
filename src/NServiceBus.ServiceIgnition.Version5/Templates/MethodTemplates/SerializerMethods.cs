namespace NServiceBus.ServiceIgnition.Version5
{
    using System;
    using System.Collections.Generic;

    public static class SerializerMethods
    {
        public static Dictionary<Serializer, Action<BusConfiguration>> MethodsDictionary = new Dictionary<Serializer, Action<BusConfiguration>>()
        {
            { Serializer.Json, Json },
            { Serializer.Xml, Xml },
            { Serializer.Binary, Binary },
        };

        public static void Json(BusConfiguration endpointConfiguration)
        {
            endpointConfiguration.UseSerialization<JsonSerializer>();
        }

        public static void Xml(BusConfiguration endpointConfiguration)
        {
            endpointConfiguration.UseSerialization<XmlSerializer>();
        }
        public static void Binary(BusConfiguration endpointConfiguration)
        {
            endpointConfiguration.UseSerialization<BinarySerializer>();
        }
    }
}