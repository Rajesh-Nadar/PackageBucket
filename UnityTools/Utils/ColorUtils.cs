using UnityEngine;

namespace UnityTools.Utils
{
    public static class ColorUtils
    {
        /// <summary>
        /// Converts a hex string (#RRGGBB or #RRGGBBAA) to a UnityEngine.Color.
        /// </summary>
        public static Color HexToColor(string hex, bool alphaInFront = false)
        {
            if (string.IsNullOrEmpty(hex))
                return Color.white;

            // Remove '#' if present
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            byte r = 255, g = 255, b = 255, a = 255;

            if (hex.Length == 6) // RGB
            {
                r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            }
            else if (hex.Length == 8) // RGBA
            {
                if (!alphaInFront)
                {
                    r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    r = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                }

            }
            else
            {
                Debug.LogWarning($"Hex string {hex} is not in the correct format (#RRGGBB or #RRGGBBAA).");
            }

            return new Color32(r, g, b, a);
        }
    }
}