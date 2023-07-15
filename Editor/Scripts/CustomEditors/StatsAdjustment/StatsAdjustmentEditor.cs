using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors {
	[CustomEditor(typeof(StatsAdjustment))]
	public class StatsAdjustmentEditor : Editor {
		private SortableListModifierGroup _modifiers;

		private SerializedProperty _propId;
		private SerializedProperty _propForceOperator;
		private SerializedProperty _propForceDisplay;
		private SerializedProperty _propAdjustments;

		public SerializedProperty PropForceDisplay => _propForceDisplay;
        public SerializedProperty PropForceOperator => _propForceOperator;

        private void OnEnable () {
			_modifiers = new SortableListModifierGroup(this, "_adjustments", "Modifiers");

			_propId = serializedObject.FindProperty("_id");
			_propForceOperator = serializedObject.FindProperty("_forceOperator");
			_propForceDisplay = serializedObject.FindProperty("_forceDisplay");
			_propAdjustments = serializedObject.FindProperty("_adjustments");
		}

		public override void OnInspectorGUI () {
			serializedObject.Update();

			EditorGUILayout.PropertyField(_propId);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_propForceDisplay);
			EditorGUILayout.PropertyField(_propForceOperator);
			if (EditorGUI.EndChangeCheck()) {
				SyncModifiers();
			}

			serializedObject.ApplyModifiedProperties();

			_modifiers.Update();
			WritePickedObject();

			serializedObject.ApplyModifiedProperties();
		}

		void WritePickedObject () {
			if (Event.current.commandName != "ObjectSelectorClosed"
			    || Event.current.type != EventType.ExecuteCommand) return;

			var id = EditorGUIUtility.GetObjectPickerControlID();
			if (id != SortableListModifierGroup.PICKER_CONTROL_ID) return;

			var obj = EditorGUIUtility.GetObjectPickerObject();
			if (obj == null || !(obj is StatDefinition)) return;

			_propAdjustments.arraySize += 1;

			var element = _propAdjustments.GetArrayElementAtIndex(_propAdjustments.arraySize - 1);
			var def = (StatDefinition)obj;
			var propDef = element.FindPropertyRelative("definition");
			propDef.objectReferenceValue = def;
		}

		void SyncModifiers () {
			for (var i = 0; i < _propAdjustments.arraySize; i++) {
				var element = _propAdjustments.GetArrayElementAtIndex(i);

				var propOperator = element.FindPropertyRelative("operatorType");
				var propOperatorEnum = (OperatorTypeNone)_propForceOperator.enumValueIndex;
				if (propOperatorEnum != OperatorTypeNone.None) {
					var operatorType = propOperatorEnum.ToOperatorType();
					propOperator.enumValueIndex = (int)operatorType;
				}

				var propValueType = element.FindPropertyRelative("value.type");
				var propValueTypeEnum = (StatValueTypeNone)_propForceDisplay.enumValueIndex;
				if (propValueTypeEnum != StatValueTypeNone.None) {
					var valueType = propValueTypeEnum.ToStatValueType();
					propValueType.enumValueIndex = (int)valueType;
				}
			}
		}
	}
}


