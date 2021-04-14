using Adnc.Utility.Editors;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors {
	public class SortableListDefinitions : SortableListBase {
		public const int PICKER_CONTROL_ID = 236534;

		public SortableListDefinitions (Editor editor, string property, string title) : base(editor, property, title) {
			_list.onAddCallback = list => {
				SelectDefinition();
			};

			_list.onRemoveCallback = list => {
				// @src http://answers.unity3d.com/questions/555724/serializedpropertydeletearrayelementatindex-leaves.html
				// Fixes a bug where Unity leaves behind empty array residue
				if (_serializedProp.GetArrayElementAtIndex(list.index) != null) {
					_serializedProp.DeleteArrayElementAtIndex(list.index);
				}

				_serializedProp.DeleteArrayElementAtIndex(list.index);
			};

			_list.drawElementCallback = (rect, index, active, focused) => {
				rect.height = EditorGUIUtility.singleLineHeight;
				GUI.enabled = false;
				EditorGUI.ObjectField(rect, _serializedProp.GetArrayElementAtIndex(index), new GUIContent(""));
				GUI.enabled = true;
			};
		}

		void SelectDefinition () {
			EditorGUIUtility.ShowObjectPicker<StatDefinitionBase>(null, false, null, PICKER_CONTROL_ID);
		}
	}
}
