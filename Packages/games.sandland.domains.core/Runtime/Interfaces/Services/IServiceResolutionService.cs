namespace Sandland.Domains.Core.Interfaces.Services
{
    /// <summary>
    /// Service that intended to resolve services registered in other domains
    /// </summary>
    public interface IServiceResolutionService : IService
    {
        TInterface ResolveService<TInterface>() where TInterface : IService;
    }
}