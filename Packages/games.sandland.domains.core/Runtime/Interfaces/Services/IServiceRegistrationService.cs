using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sandland.Domains.Core.Interfaces.Services
{
    /// <summary>
    /// Service that intended to register services that are resolvable in other domains
    /// </summary>
    public interface IServiceRegistrationService : IService
    {
        void RegisterService<TInterface, TType>(IContainerBuilder builder, LifetimeScope scope) where TType : TInterface where TInterface : IService;
        void RegisterComponentService<TInterface, TType>(IContainerBuilder builder, LifetimeScope scope, TType monoBehaviour) where TType : MonoBehaviour, TInterface where TInterface : IService;
        void UnRegisterService<TInterface>() where TInterface : IService;
        void UnRegisterService(Type serviceType);
    }
}