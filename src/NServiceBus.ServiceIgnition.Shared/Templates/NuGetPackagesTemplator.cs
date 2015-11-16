namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NuGetPackagesTemplator
    {
        private static string CreatePackageInclude(string id, string version, string targetFramework)
        {
            var template = @"<package id=""{{id}}"" version=""{{version}}"" targetFramework=""{{targetFramework}}"" />";

            var result =
                template
                    .Replace("{{id}}", id)
                    .Replace("{{version}}", version)
                    .Replace("{{targetFramework}}", targetFramework);

            return result;
        }

        public static FileAbstraction CreatePackagesFile(List<NugetDependency> dependencies)
        {
            var packageIncludes =
                dependencies
                    .Select(d => CreatePackageInclude(d.Name, d.Version, targetFramework: "452")) //TODO: choose a version
                    .ToList();

            var fileContent =
                PackagesFileTemplate
                    .Replace("{{packages}}", string.Join(Environment.NewLine, packageIncludes));

            var solutionFile = new FileAbstraction()
            {
                Name = "packages.config",
                Content = fileContent
            };

            return solutionFile;
        }

        public static FileAbstraction CreatePackagesFile(EndpointConfiguration configuration, AbstractNuGetDependencyMapper dependencyMapper)
        {
            var dependencies = dependencyMapper.GetEndpointDependencies(configuration);

            return CreatePackagesFile(dependencies);
        }

        public static string PackagesFileTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  {{packages}}
</packages>";
    }
}