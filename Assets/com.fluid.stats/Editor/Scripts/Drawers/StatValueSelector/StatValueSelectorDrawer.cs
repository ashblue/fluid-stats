using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors.Drawers {
    [CustomPropertyDrawer(typeof(StatValueSelector), true)]
    public class StatValueSelectorDrawer : StatValueSelectorBaseDrawer {
        private const float WIDTH_VALUE = 0.5f;
        private const float WIDTH_TYPE = 0.5f;
        private const float PADDING_HORIZONTAL = 5;

        private float _heightLabel;
        private float _heightEnum;
        private float _heightNumber;

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            var propType = GetPropertyType(property);
            var propValue = GetPropertyValueType(propType.enumValueIndex, property);

            EditorGUI.BeginProperty(position, label, property);

            var posLabel = position;
            posLabel.width = EditorGUIUtility.labelWidth;
            EditorGUI.LabelField(posLabel, label);

            var posValue = position;
            posValue.x += posLabel.width;
            posValue.width = (position.width - posLabel.width - PADDING_HORIZONTAL) * WIDTH_VALUE;
            EditorGUI.PropertyField(posValue, propValue, new GUIContent(""));

            var posType = position;
            posType.x += posLabel.width + posValue.width + PADDING_HORIZONTAL;
            posType.width = (position.width - posLabel.width - PADDING_HORIZONTAL) * WIDTH_TYPE;
            EditorGUI.PropertyField(posType, propType, new GUIContent(""));

            EditorGUI.EndProperty();
        }
    }
}
