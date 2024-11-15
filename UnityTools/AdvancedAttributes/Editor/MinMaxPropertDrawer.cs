using UnityEditor;
using UnityEngine;

namespace UnityTools.AdvancedAttributes
{
    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Ensure the property is a Vector2
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                MinMaxRangeAttribute range = (MinMaxRangeAttribute) attribute;
                Vector2 rangeValues = property.vector2Value;

                // Calculate layout
                float labelWidth = EditorGUIUtility.labelWidth;
                float fieldWidth = 50f;
                float sliderPadding = 5f;
                float sliderWidth = position.width - labelWidth - 2 * fieldWidth - 2 * sliderPadding;

                Rect labelRect = new Rect(position.x, position.y, labelWidth, position.height);
                Rect minFieldRect = new Rect(position.x + labelWidth, position.y, fieldWidth, position.height);
                Rect sliderRect = new Rect(position.x + labelWidth + fieldWidth + sliderPadding, position.y, sliderWidth, position.height);
                Rect maxFieldRect = new Rect(position.x + labelWidth + fieldWidth + sliderPadding + sliderWidth + sliderPadding, position.y, fieldWidth, position.height);

                // Draw the label
                EditorGUI.LabelField(labelRect, label);

                // Draw the min value field
                rangeValues.x = EditorGUI.FloatField(minFieldRect, rangeValues.x);

                // Draw the slider
                EditorGUI.MinMaxSlider(sliderRect, ref rangeValues.x, ref rangeValues.y, range.minLimit, range.maxLimit);

                // Draw the max value field
                rangeValues.y = EditorGUI.FloatField(maxFieldRect, rangeValues.y);

                // Clamp the values to ensure min <= max
                rangeValues.x = Mathf.Clamp(rangeValues.x, range.minLimit, rangeValues.y);
                rangeValues.y = Mathf.Clamp(rangeValues.y, rangeValues.x, range.maxLimit);

                // Set the property value to the modified Vector2
                property.vector2Value = rangeValues;
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use MinMaxRange with Vector2");
            }
        }
    }
}