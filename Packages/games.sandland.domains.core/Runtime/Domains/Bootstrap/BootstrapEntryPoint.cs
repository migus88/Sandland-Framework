using System.Threading;
using Cysharp.Threading.Tasks;
using Sandland.Domains.Core.Interfaces.Services;
using Sandland.Domains.Core.Interfaces.Settings;
using VContainer.Unity;

namespace Sandland.Domains.Core.Domains.Bootstrap
{
    public class BootstrapEntryPoint : IAsyncStartable
    {
        private readonly IGameLoadingSettings _gameLoadingSettings;
        private readonly IDomainService _domainService;

        public BootstrapEntryPoint(IGameLoadingSettings gameLoadingSettings, IDomainService domainService)
        {
            _gameLoadingSettings = gameLoadingSettings;
            _domainService = domainService;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            if (!string.IsNullOrEmpty(_gameLoadingSettings.CoreServices))
            {
                await _domainService.LoadDomain(_gameLoadingSettings.CoreServices, cancellation);
                cancellation.ThrowIfCancellationRequested();
            }
            
            foreach (var domain in _gameLoadingSettings.LoadingOrder)
            {
                await _domainService.LoadDomain(domain, cancellation);
                cancellation.ThrowIfCancellationRequested();
            }
        }
    }
}