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

        public NServiceBusVersion Version => NServiceBusVersion.Five;

        public BootstrappedSolution BootstrapSolution(SolutionConfiguration solutionConfig)
        {
            MarkMessagesAsEvents(solutionConfig);

            var solution = new BootstrappedSolution();
            var projects = new List<BootstrappedProject>();
            var globalDependencies = new List<ProjectReferenceData>();

            var messagesProject = GenerateMessagesProject(solutionConfig);

            var messagesProjectDependencies =
                dependencyMapper.GetDependencies(solutionConfig.NServiceBusVersion, solutionConfig.Transport);

            projects.Add(messagesProject);

            solution.ProjectDependencyDictionary.Add(messagesProject.ProjectName, messagesProjectDependencies);

            globalDependencies.Add(new ProjectReferenceData()
            {
                Name = messagesProject.ProjectName,
                ProjectGuid = messagesProject.ProjectGuid,
                QualifiedLocation = "",
            });

            foreach (var endpointConfig in solutionConfig.EndpointConfigurations)
            {
                var endpointProject = GenerateEndpoint(endpointConfig, globalDependencies);
                var dependencies = dependencyMapper.GetEndpointDependencies(endpointConfig);
                solution.ProjectDependencyDictionary.Add(endpointProject.ProjectName, dependencies);
                projects.Add(endpointProject);
            }

            var consoleApplication = GenerateConsoleProject(solutionConfig, globalDependencies);

            projects.Add(consoleApplication);

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

        private BootstrappedProject GenerateConsoleProject(SolutionConfiguration solutionConfig, List<ProjectReferenceData> projectDependencies)
        {
            var consoleProject = new BootstrappedProject()
            {
                ProjectName = TextPlaceholder.ConsoleProjectName,
                ProjectGuid = Guid.NewGuid()
            };

            var busConfigurations = new List<string>
            {
                GetMethodBody(TransportMethods.MethodsDictionary[solutionConfig.Transport]),
                GetMethodBody(SerializerMethods.MethodsDictionary[solutionConfig.Serializer]),
                GetMethodBody(PersistenceMethods.MethodsDictionary[Persistence.InMemory]),
            };

            var uniqueMessages = GetUniqueMessages(solutionConfig);

            var commandExamples =
                uniqueMessages
                    .Where(i => !i.IsEvent)
                    .Select(m =>
                        GetMethodBody(BusMethods.MethodsDictionary[BusMethod.Send]).Replace(TextPlaceholder.MessagePlaceholder, m.MessageTypeName));

            var messageExamples = new List<string>();

            messageExamples.AddRange(commandExamples);
            //messageExamples.AddRange(eventExamples);

            var programContent = GetClassTemplate<Program>();

            programContent = PlaceIndentedMultiLineText(programContent, TextPlaceholder.BusConfigurationCallsPlaceholder, busConfigurations);
            programContent = PlaceIndentedMultiLineText(programContent, TextPlaceholder.BusExampleCalls, messageExamples);

            consoleProject.ProjectRoot.Files.Add(new FileAbstraction()
            {
                Name = "Program.cs",
                Content = programContent
            });

            var nugetDependencies =
                dependencyMapper.GetDependencies(solutionConfig.NServiceBusVersion, solutionConfig.Transport);

            consoleProject.ProjectRoot.Files.Add(NuGetPackagesTemplator.CreatePackagesFile(nugetDependencies));
            consoleProject.ProjectRoot.Files.Add(AppConfigTemplator.CreateAppConfig(solutionConfig));
            consoleProject.ProjectRoot.Files.Add(new FileAbstraction() { Name = "ProvideErrorConfiguration.cs", Content = GetClassTemplate<ProvideErrorConfiguration>() });

            consoleProject.ProjectRoot.Files.Add(new FileAbstraction()
            {
                Name = consoleProject.ProjectName + ".csproj",
                Content = ProjectFileTemplator.CreateProjectFile(consoleProject, ProjectType.Exe, projectDependencies)
            });

            return consoleProject;
        }

        private BootstrappedProject GenerateMessagesProject(SolutionConfiguration solutionConfig)
        {
            var messagesProject = new BootstrappedProject()
            {
                ProjectName = TextPlaceholder.SharedProjectName,
                ProjectGuid = Guid.NewGuid()
            };

            var uniqueMessages = GetUniqueMessages(solutionConfig);

            var commandClasses =
                uniqueMessages.Where(m => !m.IsEvent).Select(m => new FileAbstraction()
                {
                    Name = m.MessageTypeName + ".cs",
                    Content = GetClassTemplate<MessagePlaceholder>().Replace(TextPlaceholder.MessagePlaceholder, m.MessageTypeName)
                });

            var eventClasses =
                uniqueMessages.Where(m => m.IsEvent).Select(m => new FileAbstraction()
                {
                    Name = m.MessageTypeName + ".cs",
                    Content = GetClassTemplate<EventPlaceholder>().Replace(TextPlaceholder.EventPlaceholder, m.MessageTypeName)
                });

            messagesProject.ProjectRoot.Files.AddRange(commandClasses);
            messagesProject.ProjectRoot.Files.AddRange(eventClasses);

            var dependencies =
                dependencyMapper.GetDependencies(solutionConfig.NServiceBusVersion, solutionConfig.Transport);

            messagesProject.ProjectRoot.Files.Add(NuGetPackagesTemplator.CreatePackagesFile(dependencies));

            messagesProject.ProjectRoot.Files.Add(new FileAbstraction()
            {
                Name = messagesProject.ProjectName + ".csproj",
                Content = ProjectFileTemplator.CreateProjectFile(messagesProject, ProjectType.Library)
            });

            return messagesProject;
        }

        private static void MarkMessagesAsEvents(SolutionConfiguration solutionConfig)
        {
            var messageReference = new Dictionary<string, List<MessageHandlerConfiguration>>();

            var messagesToCheck =
                solutionConfig.EndpointConfigurations.SelectMany(e => e.MessageHandlers);

            foreach (var message in messagesToCheck)
            {
                var key = message.MessageTypeName;

                if (messageReference.ContainsKey(key))
                {
                    messageReference[key].Add(message);
                    messageReference[key].ForEach(i => i.IsEvent = true);
                }
                else
                {
                    messageReference.Add(key, new List<MessageHandlerConfiguration>() { message });
                }
            }
        }

        private static List<MessageHandlerConfiguration> GetUniqueMessages(SolutionConfiguration solutionConfig)
        {
            var messagesToCheck =
                solutionConfig.EndpointConfigurations.SelectMany(e => e.MessageHandlers);

            var uniqueMessages = new List<MessageHandlerConfiguration>();

            foreach (var message in messagesToCheck)
            {
                if (uniqueMessages.Any(i => i.MessageTypeName == message.MessageTypeName))
                {
                    continue;
                }

                uniqueMessages.Add(message);
            }

            return uniqueMessages;
        }

        BootstrappedProject GenerateEndpoint(EndpointConfiguration endpointConfig, List<ProjectReferenceData> dependencies)
        {
            var endpointConfigTemplate = GetClassTemplate<ProgramService>();

            var project = new BootstrappedProject()
            {
                ProjectName = endpointConfig.EndpointName,
                ProjectGuid = endpointConfig.ProjectGuid
            };

            var endpointClassText =
                endpointConfigTemplate
                    .Replace(TextPlaceholder.EndpointNamePlaceholder, endpointConfig.EndpointName);

            var busConfigurations = new List<string>
            {
                GetMethodBody(TransportMethods.MethodsDictionary[endpointConfig.Transport]),
                GetMethodBody(SerializerMethods.MethodsDictionary[endpointConfig.Serializer]),
                GetMethodBody(PersistenceMethods.MethodsDictionary[endpointConfig.Persistence])
            };

            endpointClassText = PlaceIndentedMultiLineText(endpointClassText, TextPlaceholder.BusConfigurationCallsPlaceholder, busConfigurations);

            project.ProjectRoot.Files.Add(new FileAbstraction() { Name = "ProgramService.cs", Content = endpointClassText });
            project.ProjectRoot.Files.Add(new FileAbstraction() { Name = "ProvideErrorConfiguration.cs", Content = GetClassTemplate<ProvideErrorConfiguration>() });

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
            project.ProjectRoot.Files.Add(AppConfigTemplator.CreateAppConfig(endpointConfig));

            project.ProjectRoot.Folders.Add(new FolderAbstraction()
            {
                Files = messageHandlers.ToList()
            });

            //var endpointConfigTree = CSharpSyntaxTree.ParseText(endpointClassText);

            project.ProjectRoot.Files.Add(new FileAbstraction()
            {
                Name = project.ProjectName + ".csproj",
                Content = ProjectFileTemplator.CreateProjectFile(project, ProjectType.Exe, dependencies, isSelfHost: true)
            });

            return project;
        }

        private static string PlaceIndentedMultiLineText(string classText, string placeholder, List<string> lines)
        {
            var indent = GetIndent(classText, placeholder);

            var busConfigurationsText = string.Join(Environment.NewLine, lines.ToArray());

            var newLineWithSpaces = new Regex(@"\r\n *");

            busConfigurationsText = newLineWithSpaces.Replace(busConfigurationsText, Environment.NewLine + indent);

            classText =
                classText.Replace(placeholder, busConfigurationsText);

            return classText;
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
            var type = typeof(T);

            var definition = ClassDefinitionTemplates.Dictionary[type.Name];

            return definition.Replace("//# ", "");
        }

        protected string GetMethodBody(Delegate action)
        {
            var method = action.Method;

            var key = method.DeclaringType.Name + "." + method.Name;

            var methodBody = BusMethodTemplates.Dictionary[key];

            return methodBody.Replace(@"\r\n", "");
        }
    }
}