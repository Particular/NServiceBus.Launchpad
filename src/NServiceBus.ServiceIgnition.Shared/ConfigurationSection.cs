namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;

    public class ConfigurationSection<T> where T : struct, IConvertible
    {
        public string Title { get; set; }
        public string HelpText { get; set; }
        public List<ConfigurationItem<T>> Items { get; set; }
    }
}