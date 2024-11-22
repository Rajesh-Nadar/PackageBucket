using UnityEngine;

namespace UnityTools.AdvancedAttributes
{
    public enum MessageType
    {
        //
        // Summary:
        //     Neutral message.
        None,
        //
        // Summary:
        //     Info message.
        Info,
        //
        // Summary:
        //     Warning message.
        Warning,
        //
        // Summary:
        //     Error message.
        Error
    }

    public class InfoBoxAttribute : PropertyAttribute
    {
        public string message;
        public MessageType type;

        public InfoBoxAttribute (string message, MessageType type)
        {
            this.message = message;
            this.type = type;
        }
    }
}