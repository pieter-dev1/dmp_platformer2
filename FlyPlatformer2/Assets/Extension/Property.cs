using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extension
{
    public static class Property
    {
        public static object GetOppositeValue(this object value)
        {
            var valueType = value.GetType();
            if (valueType == typeof(int))
                return -(int)value;
            else if (valueType == typeof(float))
                return -(float)value;
            else if (valueType == typeof(bool))
                return !(bool)value;
            return value;
        }
    }
}
