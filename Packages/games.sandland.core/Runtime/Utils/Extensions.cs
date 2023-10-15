using System;
using System.Linq;

namespace Sandland.Core.Utils
{
    public static class Extensions
    {
        public static string ToDescription(this Enum genericEnum)
        {
            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            
            if (memberInfo.Length <= 0)
            {
                return genericEnum.ToString();
            }
            
            var attributes = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            
            return attributes.Any() ? ((System.ComponentModel.DescriptionAttribute)attributes.ElementAt(0)).Description : genericEnum.ToString();
        }
    }
}