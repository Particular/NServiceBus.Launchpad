namespace NServiceBus.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class AbstractVersionOptionBuilder
    {
        public abstract VersionConfigurationOptions GetConfigurationOptions();

        private ConfigurationItem<T> MakeConfigurationItem<T>(T item) where T : struct, IConvertible
        {
            return new ConfigurationItem<T>()
            {
                Name = item.ToString(),
                Value = item
            };
        }

        protected List<ConfigurationItem<T>> MakeConfigurationList<T>(params T[] values) where T : struct, IConvertible
        {
            return values.Select(MakeConfigurationItem).ToList();
        }
    }
}