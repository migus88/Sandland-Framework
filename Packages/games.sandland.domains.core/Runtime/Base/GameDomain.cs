using VContainer;
using VContainer.Unity;

namespace Sandland.Domains.Core.Base
{
    public abstract class GameDomain : LifetimeScope
    {
        protected sealed override void Configure(IContainerBuilder builder)
        {
            // TODO: Do some internal stuff
            ConfigureInternal(builder);
        }

        protected abstract void ConfigureInternal(IContainerBuilder builder);
    }
}