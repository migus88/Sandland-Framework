using Sandland.Core.Interfaces;
using UnityEngine;

namespace Sandland.Core.Pools
{
    public class MonoBehaviourPool<T> : BasicPool<T> where T : MonoBehaviour
    {
        protected T Prefab { get; }
        protected Transform InstantiationParent { get; }

        public MonoBehaviourPool(T prefab, Transform instantiationParent)
        {
            Prefab = prefab;
            InstantiationParent = instantiationParent;
        }
        
        protected override void OnBeforeReturningObject(ref T obj)
        {
            obj.gameObject.SetActive(false);
        }

        protected override void OnBeforeGettingObject(ref T obj)
        {
            obj.gameObject.SetActive(true);
        }

        protected override T CreateObject()
        {
            return Object.Instantiate(Prefab, InstantiationParent);
        }
    }
}