using System;
using System.Collections.Generic;
using Sandland.Domains.Core.Interfaces.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sandland.Domains.Core.Services
{
    internal class CrossDomainService : IServiceRegistrationService, IServiceResolutionService
    {
        private readonly Dictionary<Type, LifetimeScope> _typeToScopeMap = new();

        public void RegisterService<TInterface, TType>(IContainerBuilder builder, LifetimeScope scope) where TType : TInterface where TInterface : IService
        {
            var type = typeof(TInterface);
            ValidateRegisteringKey(type);
            
            _typeToScopeMap.Add(type, scope);
            builder.Register<TInterface, TType>(Lifetime.Singleton);
        }

        public void RegisterComponentService<TInterface, TType>(IContainerBuilder builder, LifetimeScope scope,
            TType monoBehaviour) where TType : MonoBehaviour, TInterface where TInterface : IService
        {
            var type = typeof(TInterface);
            ValidateRegisteringKey(type);
            
            _typeToScopeMap.Add(type, scope);
            builder.RegisterComponent<TInterface>(monoBehaviour);
        }

        public void UnRegisterService<TInterface>() where TInterface : IService
        {
            var type = typeof(TInterface);
            UnRegisterService(type);
        }

        public void UnRegisterService(Type serviceType)
        {
            _typeToScopeMap.Remove(serviceType);
        }

        public TInterface ResolveService<TInterface>() where TInterface : IService
        {
            var type = typeof(TInterface);
            var hasResolver = _typeToScopeMap.TryGetValue(type, out var scope);

            if (!hasResolver)
            {
                throw new Exception($"Service {type} not found");
            }

            if (!scope)
            {
                _typeToScopeMap.Remove(type);
                throw new Exception($"Can't resolve {type}. It's container doesn't exist or already destroyed");
            }

            return scope.Container.Resolve<TInterface>();
        }

        private void ValidateRegisteringKey(Type type)
        {
            if (_typeToScopeMap.ContainsKey(type))
            {
                throw new Exception($"Service with '{type}' type already registered");
            }
        }
    }
}