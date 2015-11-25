namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public enum ProjectType
    {
        Library,
        Exe
    }

    public class ProjectReferenceData
    {
        public string QualifiedLocation { get; set; }
        public string Name { get; set; }
        public Guid ProjectGuid { get; set; }
    }

    public class IncludesData
    {
        public IncludesData()
        {
            Compile = new List<string>();
            Content = new List<string>();
        }

        public List<string> Compile { get; set; }
        public List<string> Content { get; set; }
    }

    public class ProjectFileTemplator
    {
        private static string CreateProjectReference(ProjectReferenceData data)
        {
            var template = @"    <ProjectReference Include=""..\{{location}}{{name}}.csproj"">
      <Project>{{{guid}}}</Project>
      <Name>{{name}}</Name>
    </ProjectReference>";

            var result =
                template
                .Replace("{{name}}", data.Name)
                .Replace("{{guid}}", data.ProjectGuid.ToString())
                .Replace("{{location}}", data.QualifiedLocation);

            return Environment.NewLine + result;
        }

        public static string CreateProjectFile(BootstrappedProject project, ProjectType projectType, IEnumerable<ProjectReferenceData> references = null, bool isSelfHost = false)
        {
            var includes = CrawlFoldersForIncludes(project.ProjectRoot);
            var compileIncludesText = string.Join("", includes.Compile.ToArray());

            var contentIncludesText = "";
            var referencesText = "";

            var selfHostReferences = isSelfHost ? Environment.NewLine + @"    <Reference Include=""System.ServiceProcess"" />" : "";
            var queueConfigurationReferences = isSelfHost ? Environment.NewLine + @"    <Reference Include=""System.Configuration"" />" : "";

            if (references != null)
            {
                var projectReferences = references.Select(CreateProjectReference);
                referencesText = Environment.NewLine + "<ItemGroup>" + string.Join("", projectReferences.ToArray()) + Environment.NewLine + "</ItemGroup>";
            }

            if (includes.Content.Any())
            {
                contentIncludesText = Environment.NewLine + "<ItemGroup>" + string.Join("", includes.Content.ToArray()) + Environment.NewLine + "</ItemGroup>";
            }

            var projectFileContent =
                ClassLibraryProject
                    .Replace("{{projectGuid}}", project.ProjectGuid.ToString())
                    .Replace("{{outputType}}", projectType.ToString())
                    .Replace("{{assemblyName}}", project.ProjectName)
                    .Replace("{{compileIncludes}}", compileIncludesText)
                    .Replace("{{contentIncludes}}", contentIncludesText)
                    .Replace("{{queueConfigurationReferences}}", queueConfigurationReferences)
                    .Replace("{{selfHostReferences}}", selfHostReferences)
                    .Replace("{{referenceIncludes}}", referencesText);

            return projectFileContent;
        }

        private static string CreateCompileReference(string reference)
        {
            return Environment.NewLine + @"    <Compile Include=""" + reference + "\" />";
        }
        private static string CreateContentReference(string reference)
        {
            return Environment.NewLine + @"    <Content Include=""" + reference + "\" />";
        }

        private static IncludesData CrawlFoldersForIncludes(FolderAbstraction folder, IncludesData includes = null)
        {
            //TODO: add support for nested paths, this doesn't work yet
            includes = includes ?? new IncludesData();

            var forCompile = folder.Files.Where(f => f.Name.EndsWith(".cs"));
            var forContent = folder.Files.Where(f => !f.Name.EndsWith(".cs"));

            includes.Compile.AddRange(forCompile.Select(f => CreateCompileReference(f.Name)));
            includes.Content.AddRange(forContent.Select(f => CreateContentReference(f.Name)));

            foreach (var child in folder.Folders)
            {
                CrawlFoldersForIncludes(child, includes);
            }

            return includes;
        }


        public static string ClassLibraryProject = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""14.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{{{projectGuid}}}</ProjectGuid>
    <OutputType>{{outputType}}</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>{{assemblyName}}</RootNamespace>
    <AssemblyName>{{assemblyName}}</AssemblyName>
    <TargetFrameworkVersion>v4.5.2</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""System"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Xml.Linq"" />
    <Reference Include=""System.Data.DataSetExtensions"" />
    <Reference Include=""Microsoft.CSharp"" />
    <Reference Include=""System.Data"" />
    <Reference Include=""System.Xml"" />{{selfHostReferences}}{{queueConfigurationReferences}}
  </ItemGroup>
  <ItemGroup>{{compileIncludes}}
  </ItemGroup>{{contentIncludes}}{{referenceIncludes}}
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name=""BeforeBuild"">
  </Target>
  <Target Name=""AfterBuild"">
  </Target>
  -->
</Project>";
    }
}