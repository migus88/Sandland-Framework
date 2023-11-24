using System;
using System.Collections.Generic;
using Sandland.Core.Locks.Interfaces;

namespace Sandland.Core.Locks
{
    public class LockServiceLock : ILock
    {
        public virtual string Id { get; }
        public virtual string Category { get; }
        public ILockService LockService { get; }

        public LockServiceLock(string category, ILockService lockService, bool shouldLockImmediately = true)
        {
            Id = Guid.NewGuid().ToString();
            Category = category;
            LockService = lockService;

            if (LockService == null)
            {
                throw new Exception($"{nameof(lockService)} cannot be null");
            }

            if (shouldLockImmediately)
            {
                Lock();
            }
        }

        public void Lock() => LockService.AddLock(this);

        public void Unlock() => LockService.RemoveLock(this);

        public override string ToString() => $"{nameof(LockServiceLock)}||{Category}||{Id}";

        public void Dispose()
        {
            Unlock();
        }
    }
}