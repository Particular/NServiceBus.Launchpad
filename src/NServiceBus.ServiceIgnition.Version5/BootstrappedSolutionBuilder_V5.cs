using System.Text.RegularExpressions;

namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Version5;

    public class BootstrappedSolutionBuilder_V5 : IBuildBootstrappedSolutions
    {
        private AbstractNuGetDependencyMapper dependencyMapper = new NugetDependencyManager_V5();

        public BootstrappedSolution BootstrapSolution(SolutionConfiguration solutionConfig)
        {
            var projects = new List<BootstrappedProject>();
            var globalDependencies = new List<ProjectReferenceData>();

            var messagesProject = GenerateMessagesProject(solutionConfig);

            projects.Add(messagesProject);
            globalDependencies.Add(new ProjectReferenceData()
            {
                Name=messagesProject.ProjectName,
                ProjectGuid = messagesProject.ProjectGuid,
                QualifiedLocation = "",
            });

            foreach (var endpointConfig in solutionConfig.EndpointConfigurations)
            {
                var endpointProject = GenerateEndpoint(endpointConfig, globalDependencies);
                projects.Add(endpointProject);
            }

            var solution = new BootstrappedSolution();

            foreach (var project in projects)
            {
                var projectFolder = new FolderAbstraction()
                {
                    Name = project.ProjectName,
                    Files = project.ProjectRoot.Files,
                    Folders = project.ProjectRoot.Folders
                };

                solution.SolutionRoot.Folders.Add(projectFolder);
            }

            var solutionFile = SolutionFileTemplator.CreateSolutionFile(solutionConfig);

            solution.SolutionRoot.Files.Add(solutionFile);

            return solution;
        }

        private BootstrappedProject GenerateMessagesProject(SolutionConfiguration solutionConfig)
        {
            var messagesProject = new BootstrappedProject()
            {
                ProjectName = TextPlaceholder.SharedProjectName,
                ProjectGuid = Guid.NewGuid()
            };

            var messagesToCheck = 
                solutionConfig.EndpointConfigurations.SelectMany(e => e.MessageHandlers);

            var uniqueMessages = 
                messagesToCheck.Select(i => i.MessageTypeName).Distinct();

            var messageClasses =
                uniqueMessages.Select(m => new FileAbstraction()
                {
                    Name = m + ".cs",
                    Content = GetClassTemplate<MessagePlaceholder>().Replace(TextPlaceholder.MessagePlaceholder, m)
                });

            messagesProject.ProjectRoot.Files.AddRange(messageClasses);

            var dependencies = 
                dependencyMapper.GetDependencies(solutionConfig.NServiceBusVersion, solutionConfig.Transport);

            messagesProject.ProjectRoot.Files.Add(NuGetPackagesTemplator.CreatePackagesFile(dependencies));

            messagesProject.ProjectRoot.Files.Add(new FileAbstraction()
            {
                Name = messagesProject.ProjectName + ".csproj",
                Content = ClassLibraryProjectFileTemplator.CreateLibraryProjectFile(messagesProject)
            });

            return messagesProject;
        }

        BootstrappedProject GenerateEndpoint(EndpointConfiguration endpointConfig, List<ProjectReferenceData> dependencies)
        {
            var endpointConfigTemplate = GetClassTemplate<EndpointConfig>();

            var project = new BootstrappedProject()
            {
                ProjectName = endpointConfig.EndpointName,
                ProjectGuid = endpointConfig.ProjectGuid
            };

            var busConfigurations = new List<string>
            {
                GetMethodBody(TransportMethods.MethodsDictionary[endpointConfig.Transport]),
                GetMethodBody(SerializerMethods.MethodsDictionary[endpointConfig.Serializer]),
                GetMethodBody(PersistenceMethods.MethodsDictionary[endpointConfig.Persistence])
            };

            var indent = GetIndent(endpointConfigTemplate, TextPlaceholder.BusConfigurationCallsPlaceholder);

            var busConfigurationsText = string.Join(Environment.NewLine, busConfigurations.ToArray());

            var newLineWithSpaces = new Regex(@"\r\n *");

            busConfigurationsText = newLineWithSpaces.Replace(busConfigurationsText, Environment.NewLine + indent);

            var endpointClassText =
                endpointConfigTemplate
                    .Replace(TextPlaceholder.EndpointNamePlaceholder, endpointConfig.EndpointName)
                    .Replace(TextPlaceholder.BusConfigurationCallsPlaceholder, busConfigurationsText);

            project.ProjectRoot.Files.Add(new FileAbstraction() { Name = "EndpointConfig.cs", Content = endpointClassText });

            var messageHandlers = endpointConfig.MessageHandlers.Select(m => new FileAbstraction()
            {
                Name = m.MessageTypeName + "Handler.cs",
                Content = 
                    GetClassTemplate<MessageHandler>()
                        .Replace("MessageHandler", m.MessageTypeName + "Handler")
                        .Replace(TextPlaceholder.EndpointNamePlaceholder, endpointConfig.EndpointName)
                        .Replace(TextPlaceholder.MessagePlaceholder, m.MessageTypeName)
            });

            project.ProjectRoot.Files.Add(NuGetPackagesTemplator.CreatePackagesFile(endpointConfig, dependencyMapper));

            project.ProjectRoot.Folders.Add(new FolderAbstraction()
            {
                Files = messageHandlers.ToList()
            });

            //var endpointConfigTree = CSharpSyntaxTree.ParseText(endpointClassText);

            project.ProjectRoot.Files.Add(new FileAbstraction()
            {
                Name = project.ProjectName + ".csproj",
                Content = ClassLibraryProjectFileTemplator.CreateLibraryProjectFile(project, dependencies)
            });

            return project;
        }

        private static string GetIndent(string classTemplate, string placeholder)
        {
            var locationOfPlaceholder = classTemplate.IndexOf(placeholder);

            var stringUntilTemplate = classTemplate.Substring(0, locationOfPlaceholder);

            var withoutTrailingSpaces = stringUntilTemplate.TrimEnd(' ');

            var numberOfSpaces = stringUntilTemplate.Length - withoutTrailingSpaces.Length;

            var spaces = new String(' ', numberOfSpaces);

            return spaces;
        }

        protected string GetClassTemplate<T>()
        {
            var type = typeof (T);

            var definition = ClassDefinitionTemplates.Dictionary[type.Name];

            return definition.Replace("//# ", "");
        }

        protected string GetMethodBody(Action<BusConfiguration> action)
        {
            var method = action.Method;

            var key = method.DeclaringType.Name + "." + method.Name;

            var methodBody = BusMethodTemplates.Dictionary[key];

            return methodBody.Replace(@"\r\n", "");
        }
    }
}