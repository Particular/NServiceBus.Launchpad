using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using NuGet;
using NuGet.VisualStudio;
using NUnit.Framework;

namespace NServiceBus.ServiceIgnition.Tests
{
    [TestFixture]
    public class DefaultSolutionFixture
    {
        [Test]
        public void CreatesSolutionWithoutError()
        {
            var configuration = CreateBasicConfiguration();

            var ignitor = new BootstrappedSolutionBuilder_V5();

            var solutionData = ignitor.BootstrapSolution(configuration);

            var solutionDirectory = ProjectDirectory() + @"\GeneratedSolutions\" + Guid.NewGuid();
            var pathToNuGetExe = ProjectDirectory() + @"\NuGet.exe";

            var solutionSaver = new SolutionSaver(solutionDirectory, pathToNuGetExe);

            var solutionFile = solutionSaver.SaveSolution(solutionDirectory, solutionData);

            solutionSaver.InstallNuGetPackages(solutionDirectory, solutionData, solutionFile, pathToNuGetExe);

            Assert.IsNotNull(solutionData);
        }

        private static string ProjectDirectory()
        {
            var projectName = "NServiceBus.ServiceIgnition.Tests";
            var projectDirectory = Directory.GetCurrentDirectory();
            var resultIndex = projectDirectory.IndexOf(projectName);

            if (resultIndex != -1)
            {
                projectDirectory = projectDirectory.Substring(0, resultIndex + projectName.Length);
            }

            return projectDirectory;
        }

     
        private static SolutionConfiguration CreateBasicConfiguration()
        {
            var configuration = new SolutionConfiguration()
            {
                NServiceBusVersion = NServiceBusVersion.Five,
                Transport = Transport.Msmq,
                Serializer = Serializer.Json,
                EndpointConfigurations = new List<EndpointConfiguration>()
            };

            configuration.EndpointConfigurations.Add(new EndpointConfiguration()
            {
                NServiceBusVersion = configuration.NServiceBusVersion,
                Transport = configuration.Transport,
                Serializer = configuration.Serializer,
                Persistence = Persistence.InMemory,
                EndpointName = "SomeEndpoint",
                MessageHandlers = new List<MessageHandlerConfiguration>()
                {
                    new MessageHandlerConfiguration() {MessageTypeName = "SomeMessage"},
                    new MessageHandlerConfiguration() {MessageTypeName = "SomeEvent"},
                    new MessageHandlerConfiguration() {MessageTypeName = "BlahBlahMessage"},
                }
            });

            configuration.EndpointConfigurations.Add(new EndpointConfiguration()
            {
                NServiceBusVersion = configuration.NServiceBusVersion,
                Transport = configuration.Transport,
                Serializer = configuration.Serializer,
                Persistence = Persistence.InMemory,
                EndpointName = "SomeOtherEndpoint",
                MessageHandlers = new List<MessageHandlerConfiguration>()
                {
                    new MessageHandlerConfiguration() {MessageTypeName = "SomeEvent"},
                }
            });
            return configuration;
        }
    }
}
