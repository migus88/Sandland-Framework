using System;
using UnityEngine;

namespace Sandland.Domains.Core.Interfaces
{
    public interface IEntity
    {
        event Action Enabled;
        event Action Disabled;
        event Action Destroyed;
        
        Vector3 Position { get; }
        Quaternion Rotation { get; }
    }
}