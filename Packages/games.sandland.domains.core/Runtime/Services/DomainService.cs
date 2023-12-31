using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sandland.Domains.Core.Data;
using Sandland.Domains.Core.Interfaces.Services;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Sandland.Domains.Core.Services
{
    public class DomainService : IDomainService
    {
        private readonly Dictionary<GameDomainAddress, SceneInstance> _activeAddressableDomains = new();
        private readonly Dictionary<GameDomainAddress, UniTask> _domainLoadingTasks = new();

        public UniTask LoadDomain(GameDomainAddress address, CancellationToken cancellationToken = default)
        {
            var isDomainAlreadyLoaded = _activeAddressableDomains.ContainsKey(address);

            if (isDomainAlreadyLoaded)
            {
                return UniTask.CompletedTask;
            }

            var isDomainLoading = _domainLoadingTasks.TryGetValue(address, out var loadingTask);

            return isDomainLoading
                ? loadingTask
                : LoadDomainInternal(address, cancellationToken);
        }

        public async UniTask UnloadDomain(GameDomainAddress address, CancellationToken cancellationToken = default)
        {
            var isDomainLoaded = _activeAddressableDomains.TryGetValue(address, out var sceneInstance);

            if (!isDomainLoaded)
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await Addressables.UnloadSceneAsync(sceneInstance);
            _activeAddressableDomains.Remove(address);
        }

        private async UniTask LoadDomainInternal(GameDomainAddress address, CancellationToken cancellationToken)
        {
            var isSceneLoaded = false;
            SceneInstance sceneInstance = default;
            var completionSource = new UniTaskCompletionSource();

            try
            {
                sceneInstance = await LoadAddressableScene(address, cancellationToken, completionSource);
                isSceneLoaded = true;
            }
            catch (OperationCanceledException)
            {
                await HandleDomainLoadingCancellation(cancellationToken, isSceneLoaded, sceneInstance, completionSource);
                throw;
            }
            catch (Exception ex)
            {
                completionSource.TrySetException(ex);
                throw;
            }
            finally
            {
                _domainLoadingTasks.Remove(address);
            }

            completionSource.TrySetResult();
        }

        private async Task<SceneInstance> LoadAddressableScene(GameDomainAddress address, CancellationToken cancellationToken, UniTaskCompletionSource completionSource)
        {
            _domainLoadingTasks.Add(address, completionSource.Task);

            await Addressables.InitializeAsync();
            cancellationToken.ThrowIfCancellationRequested();

            var sceneInstance = await Addressables.LoadSceneAsync(address.ToString(), LoadSceneMode.Additive).Task;
            cancellationToken.ThrowIfCancellationRequested();

            _activeAddressableDomains.Add(address, sceneInstance);
            return sceneInstance;
        }

        private static async UniTask HandleDomainLoadingCancellation(CancellationToken cancellationToken, bool isSceneLoaded, SceneInstance sceneInstance, UniTaskCompletionSource completionSource)
        {
            if (isSceneLoaded)
            {
                await Addressables.UnloadSceneAsync(sceneInstance);
            }

            completionSource.TrySetCanceled(cancellationToken);
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}