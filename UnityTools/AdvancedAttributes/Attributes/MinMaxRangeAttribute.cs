using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityTools.AdvancedAttributes
{
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public float minLimit = 0;
        public float maxLimit = 1;

        public MinMaxRangeAttribute()
        {
            minLimit = 0;
            maxLimit = 1;
        }

        public MinMaxRangeAttribute(float minLimit, float maxLimit)
        {
            this.minLimit = minLimit;
            this.maxLimit = maxLimit;
        }
    }
}