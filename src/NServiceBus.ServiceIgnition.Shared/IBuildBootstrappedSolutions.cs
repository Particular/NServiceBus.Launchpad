namespace NServiceBus.ServiceIgnition
{
    public interface IBuildBootstrappedSolutions
    {
        BootstrappedSolution BootstrapSolution(SolutionConfiguration configuration);
    }
}