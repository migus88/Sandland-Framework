using System.Threading;
using Cysharp.Threading.Tasks;
using Sandland.Domains.Core.Data;

namespace Sandland.Domains.Core.Interfaces.Services
{
    public interface IDomainService : IService
    {
        UniTask LoadDomain(GameDomainAddress address, CancellationToken cancellationToken = default);
        UniTask UnloadDomain(GameDomainAddress address, CancellationToken cancellationToken = default);
    }
}