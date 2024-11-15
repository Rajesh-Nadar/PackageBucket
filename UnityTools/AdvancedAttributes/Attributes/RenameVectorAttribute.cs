using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.AdvancedAttributes
{
    public class RenameVectorAttribute : PropertyAttribute
    {
        public string xName;
        public string yName;
        public string zName;
        public string wName;

        public RenameVectorAttribute(string xName)
        {
            this.xName = xName;
            this.yName = "y";
            this.zName = "z";
            this.wName = "w";
        }
        public RenameVectorAttribute(string xName, string yName)
        {
            this.xName = xName;
            this.yName = yName;
            this.zName = "z";
            this.wName = "w";
        }
        public RenameVectorAttribute(string xName, string yName, string zName)
        {
            this.xName = xName;
            this.yName = yName;
            this.zName = zName;
            this.wName = "w";
        }
        public RenameVectorAttribute(string xName, string yName, string zName, string wName)
        {
            this.xName = xName;
            this.yName = yName;
            this.zName = zName;
            this.wName = wName;
        }
    }
}