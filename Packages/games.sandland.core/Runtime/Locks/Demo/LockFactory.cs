using Sandland.Core.Locks.Interfaces;
using Sandland.Core.Locks.Lockables;

namespace Sandland.Core.Locks.Demo
{
    internal static class LockFactory
    {
        public static ILock TintLock(this ILockService service)
        {
            return new LockServiceLock(nameof(ForegroundTintLockable), service);
        }
    }
}