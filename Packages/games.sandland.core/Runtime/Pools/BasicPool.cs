using System;
using System.Collections.Generic;
using Sandland.Core.Interfaces;

namespace Sandland.Core.Pools
{
    public abstract class BasicPool<T> : IPool<T>
    {
        protected Queue<T> Queue { get; } = new();

        public BasicPool(IEnumerable<T> preloadedObjects = null)
        {
            if (preloadedObjects == null)
            {
                return;
            }
            
            foreach (var obj in preloadedObjects)
            {
                ReturnObject(obj);
            }
        }

        public T GetObject()
        {
            var obj = Queue.Count == 0 ? CreateObject() : Queue.Dequeue();
            OnBeforeGettingObject(ref obj);
            return obj;
        }

        public void ReturnObject(T obj)
        {
            if (Queue.Contains(obj))
            {
                throw new Exception("Object is already in the queue");
            }

            OnBeforeReturningObject(ref obj);
            Queue.Enqueue(obj);
        }

        protected abstract void OnBeforeReturningObject(ref T obj);
        protected abstract void OnBeforeGettingObject(ref T obj);
        protected abstract T CreateObject();
    }
}