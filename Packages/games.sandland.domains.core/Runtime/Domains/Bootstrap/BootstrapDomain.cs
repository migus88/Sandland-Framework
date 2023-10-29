using Sandland.Domains.Core.Base;
using Sandland.Domains.Core.Data;
using Sandland.Domains.Core.Interfaces.Services;
using Sandland.Domains.Core.Interfaces.Settings;
using Sandland.Domains.Core.Services;
using Sandland.Domains.Core.Settings;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sandland.Domains.Core.Domains.Bootstrap
{
    public class BootstrapDomain : GameDomain
    {
        [SerializeField] private GameLoadingSettings _gameLoadingSettings;
        
        protected override void ConfigureInternal(IContainerBuilder builder)
        {
            builder.Register<DomainService>(Lifetime.Singleton).As<IDomainService>();
            builder.RegisterComponent<IGameLoadingSettings>(_gameLoadingSettings);
            builder.RegisterEntryPoint<BootstrapEntryPoint>();
        }
    }
}