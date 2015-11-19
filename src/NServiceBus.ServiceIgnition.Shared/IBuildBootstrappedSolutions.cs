namespace NServiceBus.ServiceIgnition
{
    public interface IBuildBootstrappedSolutions
    {
        NServiceBusVersion Version { get; }
        BootstrappedSolution BootstrapSolution(SolutionConfiguration configuration);
    }
}