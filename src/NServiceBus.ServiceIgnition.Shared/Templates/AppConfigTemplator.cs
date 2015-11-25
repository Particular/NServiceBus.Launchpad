namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AppConfigTemplator
    {
        private const string MappingTemplate = @"        <add Assembly=""{{assemblyName}}"" Type=""{{namespace}}.{{messageName}}"" Endpoint=""{{subscribingEndpoint}}"" />";

        private static string MakeMapping(string messageAssembly, string messageName, string endpoint)
        {
            var namespaceText = messageAssembly;

            var mapping =
                MappingTemplate
                .Replace("{{assemblyName}}", messageAssembly)
                .Replace("{{namespace}}", namespaceText)
                .Replace("{{messageName}}", messageName)
                .Replace("{{subscribingEndpoint}}", endpoint);

            return Environment.NewLine + mapping;
        }

        public static FileAbstraction CreateAppConfig(SolutionConfiguration solutionConfiguration)
        {
            var mappings = new List<string>();

            foreach (var endpoint in solutionConfiguration.EndpointConfigurations)
            {
                foreach (var message in endpoint.MessageHandlers.Where(m => !m.IsEvent))
                {
                    mappings.Add(MakeMapping(TextPlaceholder.SharedProjectName, message.MessageTypeName, endpoint.EndpointName));
                }
            }

            var mappingSection = string.Join("", mappings.ToArray());

            var appConfig = FileTemplate.Replace("{{messageMappings}}", mappingSection);

            return new FileAbstraction()
            {
                Name = "app.config",
                Content = appConfig,
            };
        }
        public static FileAbstraction CreateAppConfig(EndpointConfiguration endpoint)
        {
            var mappings = new List<string>();

            foreach (var message in endpoint.MessageHandlers.Where(m => m.IsEvent))
            {
                mappings.Add(MakeMapping(TextPlaceholder.SharedProjectName, message.MessageTypeName, TextPlaceholder.ConsoleProjectName));
            }

            var mappingSection = string.Join("", mappings.ToArray());

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