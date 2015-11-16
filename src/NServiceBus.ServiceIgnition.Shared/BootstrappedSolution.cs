namespace NServiceBus.ServiceIgnition
{
    using System;
    using System.Collections.Generic;

    public class FolderAbstraction
    {
        public FolderAbstraction()
        {
            Folders = new List<FolderAbstraction>();
            Files = new List<FileAbstraction>();
        }

        public string Name { get; set; }
        public List<FolderAbstraction> Folders { get; set; }
        public List<FileAbstraction> Files { get; set; }
    }

    public class FileAbstraction
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class BootstrappedProject
    {
        public BootstrappedProject()
        {
            ProjectGuid = Guid.NewGuid();
            ProjectRoot = new FolderAbstraction();
        }

        public FolderAbstraction ProjectRoot { get; set; }
        public string NuGetInstallers { get; set; }
        public string ProjectName { get; set; }
        public Guid ProjectGuid { get; set; }
    }

    public class BootstrappedSolution
    {
        public BootstrappedSolution()
        {
            SolutionRoot = new FolderAbstraction();
        }

        public FolderAbstraction SolutionRoot { get; set; }
        public string SolutionName { get; set; }
    }
}