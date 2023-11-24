using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sandland.Core.Locks.Interfaces
{
    public interface ILock : IDisposable
    {
        string Id { get; }
        string Category { get; }
        ILockService LockService { get; }

        void Lock();
        void Unlock();
    }
}