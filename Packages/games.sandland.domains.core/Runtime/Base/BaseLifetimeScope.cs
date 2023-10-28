using System;
using System.Collections.Generic;
using Sandland.Domains.Core.Interfaces.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sandland.Domains.Core.Base
{
    public abstract class BaseLifetimeScope : LifetimeScope
    {
        private IServiceRegistrationService _registrar;
        private IContainerBuilder _builder;
        
        private readonly List<Type> _services = new();
        
        protected sealed override void Configure(IContainerBuilder builder)
        {
            _builder = builder;
            _registrar = Parent?.Container?.Resolve<IServiceRegistrationService>(); //TODO: handle individual domain loading without parent
            
            ConfigureInternal(builder);
        }

        protected abstract void ConfigureInternal(IContainerBuilder builder);

        protected void RegisterCrossDomainService<TInterface, TType>() 
            where TType : TInterface where TInterface : IService
        {
            _registrar.RegisterService<TInterface, TType>(_builder, this);
            _services.Add(typeof(TInterface));
        }

        protected void RegisterCrossDomainComponentService<TInterface, TType>(TType monoBehaviour) 
            where TType : MonoBehaviour, TInterface where TInterface : IService
        {
            _registrar.RegisterComponentService<TInterface, TType>(_builder, this, monoBehaviour);
            _services.Add(typeof(TInterface));
        }

        protected override void OnDestroy()
        {
            foreach (var service in _services)
            {
                _registrar.UnRegisterService(service);
            }
            
            base.OnDestroy();
        }
    }
}