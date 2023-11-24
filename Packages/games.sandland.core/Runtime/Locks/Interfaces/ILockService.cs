using System.Collections.Generic;

namespace Sandland.Core.Locks.Interfaces
{
    public interface ILockService
    {
        IReadOnlyCollection<ILockable> Lockables { get; }
        
        void AddLockable(ILockable lockable);
        void RemoveLockable(ILockable lockable, bool shouldUnlock);
        
        void AddLock(ILock @lock);
        void RemoveLock(ILock @lock);
    }
}