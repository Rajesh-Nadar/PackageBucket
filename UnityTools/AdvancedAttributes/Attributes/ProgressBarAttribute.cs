using System;
using UnityEngine;

namespace UnityTools.AdvancedAttributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ProgressBarAttribute : PropertyAttribute
    {
        public float maxValue;  // Maximum value for the progress bar

        public ProgressBarAttribute(float maxValue)
        {
            this.maxValue = maxValue;
        }
    }
}