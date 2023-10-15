using Sandland.Domains.Core.Data;

namespace Sandland.Domains.Core.Interfaces.Settings
{
    public interface IGameLoadingSettings
    {
        GameDomain[] ScenesToLoad { get; set; }
    }
}