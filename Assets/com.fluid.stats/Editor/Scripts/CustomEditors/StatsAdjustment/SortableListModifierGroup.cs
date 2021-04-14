using System;
using Adnc.Utility.Editors;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors {
    public class SortableListModifierGroup : SortableListBase {
        public const int PICKER_CONTROL_ID = 398473;

        public SortableListModifierGroup (Editor editor, string property, string title) : base(editor, property, title) {
            var parent = (StatsAdjustmentEditor)editor;

            _list.onAddCallback = list => {
                SelectDefinition();
            };

            _list.onRemoveCallback = list => {
                _serializedProp.DeleteArrayElementAtIndex(list.index);
            };

            _list.drawElementCallback = (rect, index, active, focused) => {
                var element = _serializedProp.GetArrayElementAtIndex(index);

                var propDef = element.FindPropertyRelative("definition");
                var propRect = rect;
                propRect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(propRect, propDef);

                var propDisplay = element.FindPropertyRelative("value.type");
                var propValue = GetPropertyByType(element, (StatValueType)propDisplay.enumValueIndex);
                propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(propRect, propValue);

                if (parent.PropForceDisplay.enumValueIndex == 0) {
                    propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(propRect, propDisplay, new GUIContent("Value Display"));
                }

                if (parent.PropForceOperator.enumValueIndex == 0) {
                    var propOperator = element.FindPropertyRelative("operatorType");
                    propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(propRect, propOperator);
                }
            };

            _list.elementHeightCallback = index => {
                var elementCount = 2;
                if (parent.PropForceDisplay.enumValueIndex == 0) elementCount += 1;
                if (parent.PropForceOperator.enumValueIndex == 0) elementCount += 1;
                var height = (EditorGUIUtility.singleLineHeight * elementCount) + (EditorGUIUtility.standardVerticalSpacing * elementCount - 1);

                return height;
            };
        }

        void SelectDefinition () {
            EditorGUIUtility.ShowObjectPicker<StatDefinition>(null, false, null, PICKER_CONTROL_ID);
        }

        SerializedProperty GetPropertyByType (SerializedProperty prop, StatValueType type) {
            switch (type) {
                case StatValueType.Int:
                    return prop.FindPropertyRelative("value._valueInt.value");
                case StatValueType.IntCurve:
                    return prop.FindPropertyRelative("value._valueIntCurve.value");
                case StatValueType.Float:
                    return prop.FindPropertyRelative("value._valueFloat.value");
                case StatValueType.FloatCurve:
                    return prop.FindPropertyRelative("value._valueFloatCurve.value");
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }
    }
}
