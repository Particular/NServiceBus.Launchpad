namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class AbstractVersionOptionBuilder
    {
        public abstract VersionConfigurationOptions GetConfigurationOptions();
        protected Dictionary<Type, string> HelpTextDictionary { get; set; }

        protected AbstractVersionOptionBuilder()
        {
            HelpTextDictionary = new Dictionary<Type, string>();
            HelpTextDictionary.Add(typeof(Persistence), "Here is some persistance help text");
            HelpTextDictionary.Add(typeof(Serializer), "Here is some serializer help text");
            HelpTextDictionary.Add(typeof(Transport), "Here is some transport help text");
        }
          
        private ConfigurationItem<T> MakeConfigurationItem<T>(T item) where T : struct, IConvertible
        {
            return new ConfigurationItem<T>()
            {
                Name = item.ToString(),
                Value = item
            };
        }

        protected ConfigurationSection<T> MakeConfigurationSection<T>(params T[] values) where T : struct, IConvertible
        {
            var configurationItems = values.Select(MakeConfigurationItem).ToList();

            var configurationSection = new ConfigurationSection<T>()
            {
                Title = typeof(T).Name,
                Items = configurationItems,
            };

            if (HelpTextDictionary.ContainsKey(typeof(T)))
            {
                configurationSection.HelpText = HelpTextDictionary[typeof(T)];
            }

            return configurationSection;
        }
    }
}