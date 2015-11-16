namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;

    public class ConfigurationItem<T> where T : struct, IConvertible
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public List<string> RelevantLinks { get; set; }
    }
}