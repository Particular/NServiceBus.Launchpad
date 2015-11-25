namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AppConfigTemplator
    {
        private const string MappingTemplate = @"        <add Assembly=""{{assemblyName}}"" Type=""{{messageName}}"" Endpoint=""{{subscribingEndpoint}}"" />";

        private static string MakeMapping(string messageAssembly, string messageName, string endpoint)
        {
            var mapping = 
                MappingTemplate
                .Replace("{{assemblyName}}", messageAssembly)
                .Replace("{{messageName}}", messageName)
                .Replace("{{subscribingEndpoint}}", endpoint);

            return mapping;
        }

        public static FileAbstraction CreateAppConfig(SolutionConfiguration solutionConfiguration)
        {
            var mappings = new List<string>();

            foreach (var endpoint in solutionConfiguration.EndpointConfigurations)
            {
                foreach (var message in endpoint.MessageHandlers)
                {
                    mappings.Add(MakeMapping(TextPlaceholder.SharedProjectName, message.MessageTypeName, endpoint.EndpointName));
                }
            }

            var mappingSection = string.Join(Environment.NewLine, mappings.ToArray());

            var appConfig = FileTemplate.Replace("{{messageMappings}}", mappingSection);

            return new FileAbstraction()
            {
                Name = "app.config",
                Content = appConfig,
            };
        }

        public static string FileTemplate =
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
	<configSections>
      <section name=""UnicastBusConfig"" type=""NServiceBus.Config.UnicastBusConfig, NServiceBus.Core"" />
    </configSections>
    <startup> 
        <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.5.2"" />
    </startup>
    <UnicastBusConfig>
      <MessageEndpointMappings>{{messageMappings}}
      </MessageEndpointMappings>
    </UnicastBusConfig>
</configuration>";
    }
}