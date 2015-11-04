namespace NServiceBus.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IBuildBootstrappedSolutions
    {
        BootstrappedSolution BootstrapSolution(SolutionConfiguration solutionConfiguration);
    }

    public class NuGetDependencyMapper
    {
        public Dictionary<string, List<string>> DependencyMap { get; set; } 
        //public Dictionary<Persistence, List<string>> PersistenceDependencies { get; set; }
        //public Dictionary<Transport, List<string>> TransportDependencies { get; set; }
        //public Dictionary<Serializer, List<string>> SerializerDependencies { get; set; }

        public void AddDependency<T>(T value, string nugetPackageName) where T : struct, IConvertible
        {
            var key = GetMapKey(value);

            if (!DependencyMap.ContainsKey(key))
            {
                DependencyMap.Add(key, new List<string>());
            }

            DependencyMap[key].Add(nugetPackageName);
        }

        private List<string> GetDependencyEntry<T>(T value) where T : struct, IConvertible
        {
            var key = GetMapKey(value);
            List<string> dependencies = null;
            DependencyMap.TryGetValue(key, out dependencies);
            return dependencies;
        }

        static string GetMapKey<T>(T value) where T : struct, IConvertible
        {
            var typePrefix = typeof(T).AssemblyQualifiedName;
            var valueSuffix = value.ToString();
            return typePrefix + "_" + valueSuffix;
        }

        public List<string> GetEndpointDependencies(EndpointConfiguration endpointConfiguration)
        {
            var dependencies = new HashSet<string>();

            List<string> transport = GetDependencyEntry(endpointConfiguration.Transport),
                serializer = GetDependencyEntry(endpointConfiguration.Serializer),
                persistence = GetDependencyEntry(endpointConfiguration.Persistence);

            AddUniqueDependencies(transport, dependencies);
            AddUniqueDependencies(serializer, dependencies);
            AddUniqueDependencies(persistence, dependencies);

            return dependencies.ToList();
        }

        static void AddUniqueDependencies(List<string> dependenciesToAdd, HashSet<string> dependencies)
        {
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