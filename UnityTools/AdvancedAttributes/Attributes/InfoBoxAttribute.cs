using UnityEngine;

namespace UnityTools.AdvancedAttributes
{
    public class InfoBoxAttribute : PropertyAttribute
    {
        public string message;
        public UnityEditor.MessageType type;

        public InfoBoxAttribute (string message, UnityEditor.MessageType type)
        {
            this.message = message;
            this.type = type;
        }
    }
}