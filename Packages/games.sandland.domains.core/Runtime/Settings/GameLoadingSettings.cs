using System.Collections.Generic;
using Sandland.Core.Shared.Utils;
using Sandland.Domains.Core.Data;
using Sandland.Domains.Core.Interfaces.Settings;
using UnityEngine;

namespace Sandland.Domains.Core.Settings
{
    [CreateAssetMenu(fileName = nameof(GameLoadingSettings), menuName = MenuItems.Settings + nameof(GameLoadingSettings), order = 0)]
    public class GameLoadingSettings : ScriptableObject, IGameLoadingSettings
    {
        [field:SerializeField] public GameDomainAddress CoreServices { get; set; }
        [field:SerializeField] public List<GameDomainAddress> LoadingOrder { get; set; }
    }
}