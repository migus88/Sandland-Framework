using System.Collections.Generic;
using Sandland.Domains.Core.Data;

namespace Sandland.Domains.Core.Interfaces.Settings
{
    public interface IGameLoadingSettings
    {
        GameDomainAddress CoreServices { get; }
        List<GameDomainAddress> LoadingOrder { get; }
    }
}