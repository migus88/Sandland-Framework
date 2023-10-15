using System;
using UnityEngine;

namespace Sandland.Domains.Core.Data
{
    [Serializable]
    public class GameDomain
    {
        [field:SerializeField] public string Value { get; set; }

        public GameDomain() { }
        public GameDomain(string value)
        {
            Value = value;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return Value == ((GameDomain)obj).Value;
        }
    
        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        public override string ToString() => Value;

        public static implicit operator string(GameDomain domain) => domain.ToString();
        public static implicit operator GameDomain(string value) => new(value);
    }
}