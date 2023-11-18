using System;

namespace Sandland.Core.Pools
{
    public abstract class BasicDisposablePool<T> : BasicPool<T>, IDisposable where T : IDisposable
    {
        public void Dispose()
        {
            foreach (var disposable in Queue)
            {
                disposable?.Dispose();
            }
        }
    }
}