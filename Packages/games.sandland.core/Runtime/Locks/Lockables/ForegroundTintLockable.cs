using System.Collections.Generic;
using Sandland.Core.Locks.Interfaces;
using UnityEngine;

namespace Sandland.Core.Locks.Lockables
{
    public class ForegroundTintLockable : MonoBehaviour, ILockable
    {
        public string Category => nameof(ForegroundTintLockable);
        public List<ILock> Locks { get; } = new();
        
        [SerializeField] private GameObject _tint;
        
        public void Lock()
        {
            _tint.SetActive(true);
        }

        public void Unlock()
        {
            _tint.SetActive(false);
        }
    }
}