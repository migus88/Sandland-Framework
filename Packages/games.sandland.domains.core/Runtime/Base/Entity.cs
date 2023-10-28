using System;
using Sandland.Domains.Core.Interfaces;
using UnityEngine;

namespace Sandland.Domains.Core.Base
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        public event Action Enabled;
        public event Action Disabled;
        public event Action Destroyed;
        
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        
        private void OnEnable()
        {
            Enabled?.Invoke();
            OnEnableInternal();
        }

        private void OnDisable()    
        {
            Disabled?.Invoke();
            OnDisableInternal();
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
            OnDestroyInternal();
        }

        protected virtual void OnEnableInternal() { }
        protected virtual void OnDisableInternal() { }
        protected virtual void OnDestroyInternal() { }

        public virtual void Dispose() { }
    }
}