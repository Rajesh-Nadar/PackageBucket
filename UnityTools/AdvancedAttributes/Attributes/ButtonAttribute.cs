using System;
using UnityEngine;

namespace UnityTools.AdvancedAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ButtonAttribute : Attribute
    { 
        public string ButtonName { get; private set; }

        public ButtonAttribute(string buttonName = null)
        {
            ButtonName = buttonName;
        }
    }
}