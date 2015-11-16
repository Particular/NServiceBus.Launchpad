namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AbstractNuGetDependencyMapper
    {
        protected AbstractNuGetDependencyMapper()
        {
            DependencyMap = new Dictionary<string, List<NugetDependency>>();
        }

        Dictionary<string, List<NugetDependency>> DependencyMap { get; set; }

        protected void AddDependency<T>(T value, string name, string version) where T : struct, IConvertible
        {
            var key = GetMapKey(value);

            if (!DependencyMap.ContainsKey(key))
            {
                DependencyMap.Add(key, new List<NugetDependency>());
            }

            DependencyMap[key].Add(new NugetDependency() { Name = name, Version = version });
        }

        private List<NugetDependency> GetDependencyEntry<T>(T value) where T : struct, IConvertible
        {
            var key = GetMapKey(value);
            List<NugetDependency> dependencies;
            DependencyMap.TryGetValue(key, out dependencies);
            return dependencies;
        }
        public List<NugetDependency> GetDependencies<T1, T2>(T1 t1, T2 t2) where T1 : struct, IConvertible where T2 : struct, IConvertible
        {
            var dependencies = new HashSet<NugetDependency>();

            List<NugetDependency>
                transport = GetDependencyEntry(t1),
                version = GetDependencyEntry(t2);

            AddUniqueDependencies(version, dependencies);
            AddUniqueDependencies(transport, dependencies);

            return dependencies.ToList();
        }

        private static string GetMapKey<T>(T value) where T : struct, IConvertible
        {
            var typePrefix = typeof(T).AssemblyQualifiedName;
            var valueSuffix = value.ToString();
            return typePrefix + "_" + valueSuffix;
        }

        public List<NugetDependency> GetEndpointDependencies(EndpointConfiguration endpointConfiguration)
        {
            var dependencies = new HashSet<NugetDependency>();

            List<NugetDependency>
                transport = GetDependencyEntry(endpointConfiguration.Transport),
                version = GetDependencyEntry(endpointConfiguration.NServiceBusVersion),
                serializer = GetDependencyEntry(endpointConfiguration.Serializer),
                persistence = GetDependencyEntry(endpointConfiguration.Persistence);

            AddUniqueDependencies(version, dependencies);
            AddUniqueDependencies(transport, dependencies);
            AddUniqueDependencies(serializer, dependencies);
            AddUniqueDependencies(persistence, dependencies);

            return dependencies.ToList();
        }

        //public List<NugetDependency> GetSolutionDependencies(SolutionConfiguration solution)
        //{
        //    var dependencies = new HashSet<NugetDependency>();
            
        //    foreach (var endpoint in solution.EndpointConfigurations)
        //    {
        //        AddUniqueDependencies(GetEndpointDependencies(endpoint), dependencies);
        //    }

        //    return dependencies.ToList();
        //}

        private static void AddUniqueDependencies(List<NugetDependency> dependenciesToAdd, HashSet<NugetDependency> dependencies)
        {
            if (dependenciesToAdd == null)
            {
                return;
            }

            foreach (var dependency in dependenciesToAdd)
            {
                if (!dependencies.Contains(dependency))
                {
                    dependencies.Add(dependency);
                }
            }
        }
    }
}