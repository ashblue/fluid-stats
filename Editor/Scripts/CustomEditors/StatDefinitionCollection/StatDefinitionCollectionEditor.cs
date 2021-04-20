using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors {
    [CustomEditor(typeof(StatDefinitionCollection))]
    public class StatDefinitionCollectionEditor : Editor {
        private SortableListDefinitions _definitions;
        private SerializedProperty _propDefinitions;

        private StatDefinitionCollection Target => (StatDefinitionCollection)target;

        private void OnEnable () {
            _propDefinitions = serializedObject.FindProperty("definitions");
            _definitions = new SortableListDefinitions(this, "definitions", "Definitions");

            // Remove null definitions and duplicates upon inspection
            Target.definitions = Target.definitions.FindAll(d => d != null)
                .GroupBy(d => d.name)
                .Select(d => d.First())
                .ToList();
        }

        public override void OnInspectorGUI () {
            serializedObject.Update();

            _definitions.Update();
            WritePickedObject();

            serializedObject.ApplyModifiedProperties();
        }

        void WritePickedObject () {
            if (Event.current.commandName != "ObjectSelectorClosed"
                || Event.current.type != EventType.ExecuteCommand) return;

            var id = EditorGUIUtility.GetObjectPickerControlID();
            if (id != SortableListDefinitions.PICKER_CONTROL_ID) return;

            var obj = EditorGUIUtility.GetObjectPickerObject();
            if (obj == null || !(obj is StatDefinitionBase)) return;

            var def = (StatDefinitionBase)obj;
            if (Target.definitions.Contains(def)) return;

            _propDefinitions.arraySize += 1;
            _propDefinitions.GetArrayElementAtIndex(_propDefinitions.arraySize - 1).objectReferenceValue = def;
        }
    }
}
