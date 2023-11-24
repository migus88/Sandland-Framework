using System.Collections.Generic;
using Sandland.Core.Locks.Interfaces;
using UnityEngine;

namespace Sandland.Core.Locks.Lockables
{
    public class VisibilityLockableBehaviour : MonoBehaviour, ILockable
    {
        [field:SerializeField] public string Category { get; set; }
        public List<ILock> Locks { get; } = new();
        
        public void Lock()
        {
            gameObject.SetActive(false);
        }

        public void Unlock()
        {
            gameObject.SetActive(true);
        }
    }
}