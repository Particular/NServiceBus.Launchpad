namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Linq;

    public class SolutionFileTemplator
    {
        private static string CreateInstructionsFolder(Guid solutionGuid)
        {
            var template =
@"Project(""{{2150E333-8FDC-42A3-9474-1A3956D46DE8}}"") = ""_Instructions"", ""_Instructions"", ""{{{projectGuid}}}""
	ProjectSection(SolutionItems) = preProject
		README.md = README.md
	EndProjectSection
EndProject";

            var result =
                template
                    .Replace("{{projectGuid}}", Guid.NewGuid().ToString());

            return result;
        }

        private static string CreateProjectInclude(Guid solutionGuid, Guid projectGuid, string projectName)
        {
            var template = @"Project(""{{{solutionGuid}}}"") = ""{{projectName}}"", ""{{projectName}}\{{projectName}}.csproj"", ""{{{projectGuid}}}""
EndProject";

            var result =
                template
                    .Replace("{{projectName}}", projectName)
                    .Replace("{{solutionGuid}}", solutionGuid.ToString())
                    .Replace("{{projectGuid}}", projectGuid.ToString());

            return result;
        }

        private static string CreateProjectConfiguration(Guid projectGuid)
        {
            var template = @"		{{{projectGuid}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{{projectGuid}}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{{projectGuid}}}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{{{projectGuid}}}.Release|Any CPU.Build.0 = Release|Any CPU";

            var result = template.Replace("{{projectGuid}}", projectGuid.ToString());

            return result;
        }

        public static FileAbstraction CreateSolutionFile(SolutionConfiguration configuration)
        {
            var solutionName = "Ignited.NServiceBus" + configuration.NServiceBusVersion + "." + configuration.Transport.ToString();
            var solutionGuid = Guid.NewGuid();

            var projectIncludes =
                configuration.EndpointConfigurations
                    .Select(e => CreateProjectInclude(solutionGuid, e.ProjectGuid, e.EndpointName))
                    .ToList();

            var sharedProjectGuid = Guid.NewGuid();
            var consoleProjectGuid = Guid.NewGuid();

            projectIncludes.Add(CreateProjectInclude(solutionGuid, sharedProjectGuid, TextPlaceholder.SharedProjectName));
            projectIncludes.Add(CreateProjectInclude(solutionGuid, consoleProjectGuid, TextPlaceholder.ConsoleProjectName));

            var projectConfigurations =
                configuration.EndpointConfigurations
                    .Select(e => CreateProjectConfiguration(e.ProjectGuid))
                    .ToList();

            projectConfigurations.Add(CreateProjectConfiguration(sharedProjectGuid));
            projectConfigurations.Add(CreateProjectConfiguration(consoleProjectGuid));

            var solutionFileContent =
                SolutionFileTemplate
                    .Replace("{{projectIncludes}}", string.Join(Environment.NewLine, projectIncludes))
                    //.Replace("{{instructionsIncludes}}", CreateInstructionsFolder(solutionGuid))
                    .Replace("{{projectConfigurations}}", string.Join(Environment.NewLine, projectConfigurations));

            var solutionFile = new FileAbstraction()
            {
                Name = solutionName + ".sln",
                Content = solutionFileContent
            };

            return solutionFile;
        }

        public static string SolutionFileTemplate = 
@"Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 14
VisualStudioVersion = 14.0.23107.0
MinimumVisualStudioVersion = 10.0.40219.1
{{projectIncludes}}
{{instructionsIncludes}}
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
{{projectConfigurations}}
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal";
    }
}