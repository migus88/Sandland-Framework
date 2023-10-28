using System;
using UnityEngine;

namespace Sandland.Domains.Core.Data
{
    [Serializable]
    public class GameDomainAddress
    {
        [field:SerializeField] public string Value { get; set; }

        public GameDomainAddress() { }
        public GameDomainAddress(string value)
        {
            Value = value;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return Value == ((GameDomainAddress)obj).Value;
        }
    
        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        public override string ToString() => Value;

        public static implicit operator string(GameDomainAddress domainAddress) => domainAddress.ToString();
        public static implicit operator GameDomainAddress(string value) => new(value);
    }
}