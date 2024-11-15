using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityTools.AdvancedAttributes
{
    [CustomPropertyDrawer(typeof(RenameVectorAttribute))]
    public class RenameVectorPropertyDrawer : PropertyDrawer
    {
        bool multiline = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // cast the attribute to make life easier
            RenameVectorAttribute renameVector = attribute as RenameVectorAttribute;

            float xValue = 0;
            float yValue = 0;
            float zValue = 0;
            float wValue = 0;

            GetVectorValues(property, ref xValue, ref yValue, ref zValue, ref wValue);

            Rect labelPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            float propertySize = labelPosition.width / 4;

            if (propertySize > 0)
            {
                if (propertySize <= 50)
                    multiline = true;
                else
                    multiline = false;
            }

            float lineY = position.y + ((multiline ? EditorGUIUtility.singleLineHeight : 0));
            float valuesStartPosition = multiline ? position.x : labelPosition.x;
            float valuesLineSize = multiline ? position.width : labelPosition.width;

            float labelXSize = EditorStyles.label.CalcSize(new GUIContent(renameVector.xName)).x + 10;
            float labelYSize = EditorStyles.label.CalcSize(new GUIContent(renameVector.yName)).x + 10;
            float labelZSize = EditorStyles.label.CalcSize(new GUIContent(renameVector.zName)).x + 10;
            float labelWSize = EditorStyles.label.CalcSize(new GUIContent(renameVector.wName)).x + 10;

            float valueFieldSize = 0;

            if (property.propertyType == SerializedPropertyType.Vector2 || property.propertyType == SerializedPropertyType.Vector2Int)
                valueFieldSize = ((valuesLineSize - labelXSize - labelYSize) / 2) - 7;

            else if (property.propertyType == SerializedPropertyType.Vector3 || property.propertyType == SerializedPropertyType.Vector3Int)
                valueFieldSize = ((valuesLineSize - labelXSize - labelYSize - labelZSize) / 3) - 6;

            else if (property.propertyType == SerializedPropertyType.Vector4)
                valueFieldSize = ((valuesLineSize - labelXSize - labelYSize - labelZSize - labelWSize) / 4) - 6;

            (Rect xLabelRect, Rect xValueRect) = GetValueRect(valuesStartPosition, lineY, labelXSize, valueFieldSize);
            (Rect yLabelRect, Rect yValueRect) = GetValueRect(xValueRect, lineY, labelYSize, valueFieldSize);
            (Rect zLabelRect, Rect zValueRect) = GetValueRect(yValueRect, lineY, labelZSize, valueFieldSize);
            (Rect wLabelRect, Rect wValueRect) = GetValueRect(zValueRect, lineY, labelWSize, valueFieldSize);

            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                xValue = DrawVectorValue(renameVector.xName, xValue, xLabelRect, xValueRect);
                yValue = DrawVectorValue(renameVector.yName, yValue, yLabelRect, yValueRect);

                var vec = Vector2.zero; // save the results into the property!
                vec.x = xValue;
                vec.y = yValue;

                property.vector2Value = vec;
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                xValue = DrawVectorIntValue(renameVector.xName, (int) xValue, xLabelRect, xValueRect);
                yValue = DrawVectorIntValue(renameVector.yName, (int) yValue, yLabelRect, yValueRect);

                var vec = Vector2Int.zero; // save the results into the property!
                vec.x = (int) xValue;
                vec.y = (int) yValue;

                property.vector2IntValue = vec;
            }
            else if (property.propertyType == SerializedPropertyType.Vector3)
            {
                xValue = DrawVectorValue(renameVector.xName, xValue, xLabelRect, xValueRect);
                yValue = DrawVectorValue(renameVector.yName, yValue, yLabelRect, yValueRect);
                zValue = DrawVectorValue(renameVector.zName, zValue, zLabelRect, zValueRect);

                var vec = Vector3.zero; // save the results into the property!
                vec.x = xValue;
                vec.y = yValue;
                vec.z = zValue;

                property.vector3Value = vec;
            }
            else if (property.propertyType == SerializedPropertyType.Vector3Int)
            {
                xValue = DrawVectorIntValue(renameVector.xName, (int) xValue, xLabelRect, xValueRect);
                yValue = DrawVectorIntValue(renameVector.yName, (int) yValue, yLabelRect, yValueRect);
                zValue = DrawVectorIntValue(renameVector.zName, (int) zValue, zLabelRect, zValueRect);

                var vec = Vector3Int.zero; // save the results into the property!
                vec.x = (int) xValue;
                vec.y = (int) yValue;
                vec.z = (int) zValue;

                property.vector3IntValue = vec;
            }
            else if (property.propertyType == SerializedPropertyType.Vector4)
            {
                xValue = DrawVectorValue(renameVector.xName, xValue, xLabelRect, xValueRect);
                yValue = DrawVectorValue(renameVector.yName, yValue, yLabelRect, yValueRect);
                zValue = DrawVectorValue(renameVector.zName, zValue, zLabelRect, zValueRect);
                wValue = DrawVectorValue(renameVector.wName, wValue, wLabelRect, wValueRect);

                var vec = Vector4.zero; // save the results into the property!
                vec.x = xValue;
                vec.y = yValue;
                vec.z = zValue;
                vec.w = wValue;

                property.vector4Value = vec;
            }

            if (multiline)
                GUI.Box(new Rect(position.x, lineY, position.width, EditorGUIUtility.singleLineHeight), "");
        }

        private float DrawVectorValue(string name, float value, Rect labelRect, Rect valueRect)
        {
            EditorGUI.LabelField(labelRect, name, GUI.skin.name);
            value = EditorGUI.FloatField(valueRect, value);

            return value;
        }
        private float DrawVectorIntValue(string name, int value, Rect labelRect, Rect valueRect)
        {
            EditorGUI.LabelField(labelRect, name, GUI.skin.name);
            value = EditorGUI.IntField(valueRect, value);

            return value;
        }
        private (Rect, Rect) GetValueRect(float xPosition, float yPosition, float labelSize, float valueSize)
        {
            var label = new Rect(
                xPosition,
                yPosition,
                labelSize,
                EditorGUIUtility.singleLineHeight);

            var value = new Rect(
                10 + label.x + label.width,
                yPosition,
                valueSize,
                EditorGUIUtility.singleLineHeight);

            return (label, value);
        }
        private (Rect, Rect) GetValueRect(Rect rect, float yPosition, float labelSize, float valueSize)
        {
            var label = new Rect(
                5 + rect.x + rect.width,
                yPosition,
                labelSize,
                EditorGUIUtility.singleLineHeight);

            var value = new Rect(
                label.x + label.width,
                yPosition,
                valueSize,
                EditorGUIUtility.singleLineHeight);

            return (label, value);
        }

        private static void GetVectorValues(SerializedProperty property, ref float xValue, ref float yValue, ref float zValue, ref float wValue)
        {
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                xValue = property.vector2Value.x;
                yValue = property.vector2Value.y;
            }
            else if (property.propertyType == SerializedPropertyType.Vector3)
            {
                xValue = property.vector3Value.x;
                yValue = property.vector3Value.y;
                zValue = property.vector3Value.z;
            }
            else if (property.propertyType == SerializedPropertyType.Vector4)
            {
                xValue = property.vector4Value.x;
                yValue = property.vector4Value.y;
                zValue = property.vector4Value.z;
                wValue = property.vector4Value.w;
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                xValue = property.vector2IntValue.x;
                yValue = property.vector2IntValue.y;
            }
            else if (property.propertyType == SerializedPropertyType.Vector3Int)
            {
                xValue = property.vector3IntValue.x;
                yValue = property.vector3IntValue.y;
                zValue = property.vector3IntValue.z;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // by default just return the standard line height
            float size = EditorGUIUtility.singleLineHeight;

            if (multiline)
                size *= 2;

            return size;
        }
    }
}