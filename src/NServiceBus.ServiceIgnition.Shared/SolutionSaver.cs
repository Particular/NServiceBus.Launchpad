using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace NServiceBus.ServiceIgnition
{
    public class SolutionSaver
    {
        private readonly string SavePath;
        private readonly string NuGetExe;

        public SolutionSaver(string savePath, string nugetExePath)
        {
            SavePath = savePath;
            NuGetExe = nugetExePath;
        }

        public string CreateSolution(IBuildBootstrappedSolutions bootstrapper, SolutionConfiguration configuration)
        {
            configuration.InCodeSubscriptions = true;
            foreach (var endpointConfiguration in configuration.EndpointConfigurations)
            {
                endpointConfiguration.InCodeSubscriptions = configuration.InCodeSubscriptions;
            }

            var solutionData = bootstrapper.BootstrapSolution(configuration);

            var solutionDirectory = SavePath + Guid.NewGuid();

            var solutionFile = SaveSolution(solutionDirectory, solutionData);

            InstallNuGetPackages(solutionDirectory, solutionData, solutionFile, NuGetExe);

            var zipFilePath = solutionFile.Replace(".sln", ".zip");

            using (var zip = new ZipFile())
            {
                zip.AddDirectory(solutionDirectory, "rootInZipFile");
                zip.Save(zipFilePath);
            }

            return zipFilePath;
        }

        public string SaveSolution(string savePath, BootstrappedSolution solutionData)
        {
            Directory.CreateDirectory(savePath);

            solutionData.SolutionRoot.Files.Add(new FileAbstraction()
            {
                Name = "README.md",
                Content = ReadMeFile
            });

            CrawlAndSaveFiles(savePath + @"\", solutionData.SolutionRoot);

            var solutionFile = Directory.GetFiles(savePath).Single(fn => fn.EndsWith(".sln"));

            return solutionFile;
        }

        public void InstallNuGetPackages(string solutionFolder, BootstrappedSolution solution, string solutionFile, string nugetExePath)
        {
            //var nugetPsmModule = solutionFolder + @"\lib\nuget.psm1";

            //RunPowershell("Update-Package " + solutionFile + " -Reinstall", nugetPsmModule);

            var solutionNugetExe = solutionFolder + @"\NuGet.exe";
            File.Copy(nugetExePath, solutionNugetExe);

            ExecuteSilentCommandLineProcess(solutionNugetExe, "restore " + solutionFile);
            ExecuteSilentCommandLineProcess(solutionNugetExe, "install " + solutionFile); //This does NOTHING

            //var packageConfigs = CrawlAndGrabPackageConfigs(solutionFolder + @"\");

            //foreach (var packageConfig in packageConfigs)
            //{
            //    var outputDirectory = packageConfig.Replace(@"packages.config", "");
            //    var commandOptions = "install " + packageConfig + " -OutputDirectory " + outputDirectory;
            //    ExecuteSilentCommandLineProcess(nugetExe, commandOptions);
            //}


            ////Connect to the official package repository
            //var repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");

            //var projectKeys = solution.ProjectDependencyDictionary.Keys;

            //foreach (var projectKey in projectKeys)
            //{
            //    var projectFolder = solutionFolder + @"\" + projectKey + @"\";
            //    var dependencies = solution.ProjectDependencyDictionary[projectKey];

            //    var packageManager = new PackageManager(repo, projectFolder);

            //    foreach (var nugetDependency in dependencies)
            //    {
            //        packageManager.InstallPackage(nugetDependency.Name, SemanticVersion.Parse(nugetDependency.Version));
            //        //var commandOptions = "install " + nugetDependency.Name + " -Version " + nugetDependency.Version;
            //        //ExecuteSilentCommandLineProcess(nugetExe, commandOptions + " -OutputDirectory " + projectFolder);
            //    }
            //}
        }

        //private static void RunPowershell(string command, string nugetPowershellModule)
        //{
        //    var ps = PowerShell.Create();
        //    ps.Commands.AddCommand("Import-Module").AddArgument(nugetPowershellModule);
        //    ps.Invoke();
        //    ps.Commands.AddCommand(command);
        //    ps.Invoke();
        //}


        const string packageReferenceTemplate = @"    <Reference Include=""NServiceBus.Core, Version=5.0.0.0, Culture=neutral, PublicKeyToken=9fc386479f8a226c, processorArchitecture=MSIL"">
      <HintPath>..\packages\{{name}}.{{version}}\lib\net45\NServiceBus.Core.dll</HintPath>
      <Private>True</Private>
    </Reference>";

        private static void AddReferencesToNugetPackages(string solutionFolder)
        {

        }

        private static void ExecuteSilentCommandLineProcess(string nugetExe, string commandOptions)
        {
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

        private static List<string> CrawlAndGrabPackageConfigs(string path, List<string> packagePaths = null)
        {
            packagePaths = packagePaths ?? new List<string>();

            var files = Directory.GetFiles(path);
            var folders = Directory.GetDirectories(path);

            packagePaths.AddRange(files.Where(f => f.EndsWith(@"\packages.config")));

            foreach (var child in folders)
            {
                CrawlAndGrabPackageConfigs(child + @"\", packagePaths);
            }

            return packagePaths;
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

        public static string ReadMeFile =
@"# Service Ignitor in Particular

### Instructions:
 - Open the Package Manager Console and run `Update-Package -Reinstall`
 - Build the solution
 - Enjoy";
    }
}