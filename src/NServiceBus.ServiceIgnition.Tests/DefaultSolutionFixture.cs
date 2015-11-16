using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var solutionFile = SaveSolution(solutionData);

            InstallNuGetPackages(solutionFile);

            Assert.IsNotNull(solutionData);
        }

        private static string SaveSolution(BootstrappedSolution solutionData)
        {
            var savePath = @"C:\Git\ServiceIgnition\src\NServiceBus.ServiceIgnition.Tests\GeneratedSolutions\" +
                           Guid.NewGuid();

            Directory.CreateDirectory(savePath);

            CrawlAndSaveFiles(savePath + @"\", solutionData.SolutionRoot);

            var solutionFile = Directory.GetFiles(savePath).Single(fn => fn.EndsWith(".sln"));
            return solutionFile;
        }

        private static void InstallNuGetPackages(string solutionFile)
        {
            var nugetExe = @"C:\Git\ServiceIgnition\src\NServiceBus.ServiceIgnition.Tests\NuGet.exe";

            var commandOptions = " restore " + solutionFile;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = nugetExe,
                Arguments = commandOptions
            };
            process.StartInfo = startInfo;
            process.Start();
        }

        private static void CrawlAndSaveFiles(string path, FolderAbstraction folder)
        {
            foreach (var file in folder.Files)
            {
                File.WriteAllText(path + file.Name, file.Content);
            }

            foreach (var child in folder.Folders)
            {
                var newDirectory = path + child.Name;
                Directory.CreateDirectory(newDirectory);
                CrawlAndSaveFiles(newDirectory + @"\", child);
            }
        }

        private static SolutionConfiguration CreateBasicConfiguration()
        {
            var configuration = new SolutionConfiguration()
            {
                NServiceBusVersion = NServiceBusVersion.Five,
                Transport = Transport.Msmq,
                EndpointConfigurations = new List<EndpointConfiguration>()
            };

            configuration.EndpointConfigurations.Add(new EndpointConfiguration()
            {
                NServiceBusVersion = configuration.NServiceBusVersion,
                Transport = configuration.Transport,
                Serializer = Serializer.Json,
                EndpointName = "SomeEndpoint",
                Persistence = Persistence.None,
                MessageHandlers = new List<MessageHandlerConfiguration>()
                {
                    new MessageHandlerConfiguration() {MessageTypeName = "SomeMessage"},
                    new MessageHandlerConfiguration() {MessageTypeName = "SomeOtherMessage"},
                    new MessageHandlerConfiguration() {MessageTypeName = "BlahblahMessage"},
                }
            });

            configuration.EndpointConfigurations.Add(new EndpointConfiguration()
            {
                NServiceBusVersion = configuration.NServiceBusVersion,
                Transport = configuration.Transport,
                Serializer = Serializer.Json,
                EndpointName = "SomeOtherEndpoint",
                Persistence = Persistence.None,
                MessageHandlers = new List<MessageHandlerConfiguration>()
                {
                    new MessageHandlerConfiguration() {MessageTypeName = "SomeOtherMessage"},
                }
            });
            return configuration;
        }
    }
}
