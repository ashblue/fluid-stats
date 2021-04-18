using Adnc.Utility.Editors;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors {
    public class SortableListOperators : SortableListBase {
        public SortableListOperators (Editor editor, string property, string title) : base(editor, property, title) {
            _list.drawElementCallback = (rect, index, active, focused) => {
                var element = _serializedProp.GetArrayElementAtIndex(index);

                var propType = element.FindPropertyRelative("_type");
                var propAutoRound = element.FindPropertyRelative("_modifierAutoRound");

                GUI.enabled = false;
                EditorGUI.PropertyField(rect, propType, new GUIContent(""));
                GUI.enabled = true;

                var rect2 = rect;
                rect2.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect2, propAutoRound);
            };

            _list.elementHeightCallback = index => {
                return EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight * 2;
            };

            _list.onCanAddCallback = list => false;
            _list.onCanRemoveCallback = _list => false;
        }
    }
}
