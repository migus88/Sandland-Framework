using Sandland.Domains.Core.Base;
using Sandland.Domains.Core.Interfaces.Services;
using Sandland.Domains.Core.Services;
using UnityEngine;
using VContainer;

namespace Sandland.Domains.Core.Domains.Bootstrap
{
    public class CoreServicesDomain : GameDomain
    {
        [SerializeField] private int _randomSeed;
        
        protected override void ConfigureInternal(IContainerBuilder builder)
        {
            builder.Register<RandomService>(Lifetime.Singleton).As<IRandomService>().WithParameter(_randomSeed);
            // TODO: More core services registrations
        }
    }
}