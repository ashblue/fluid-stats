using System;
using UnityEditor;

namespace CleverCrow.Fluid.StatsSystem.Editors.Drawers {
	public abstract class StatValueSelectorBaseDrawer : PropertyDrawer {
		protected SerializedProperty GetPropertyValueType (int enumIndex, SerializedProperty parent) {
			var enumType = (StatValueType)enumIndex;
			return GetPropertyValueType(enumType, parent);
		}

		protected SerializedProperty GetPropertyValueType (StatValueType type, SerializedProperty parent) {
			switch (type) {
				case StatValueType.Int:
					return GetPropertyInt(parent);
				case StatValueType.IntCurve:
					return GetPropertyIntCurve(parent);
				case StatValueType.Float:
					return GetPropertyFloat(parent);
				case StatValueType.FloatCurve:
					return GetPropertyFloatCurve(parent);
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

		protected SerializedProperty GetPropertyType (SerializedProperty property) {
			return property.FindPropertyRelative("type");
		}

		protected SerializedProperty GetPropertyInt (SerializedProperty property) {
			return property.FindPropertyRelative("_valueInt").FindPropertyRelative("value");
		}

		protected SerializedProperty GetPropertyIntCurve (SerializedProperty property) {
			return property.FindPropertyRelative("_valueIntCurve").FindPropertyRelative("value");
		}

		protected SerializedProperty GetPropertyFloat (SerializedProperty property) {
			return property.FindPropertyRelative("_valueFloat").FindPropertyRelative("value");
		}

		protected SerializedProperty GetPropertyFloatCurve (SerializedProperty property) {
			return property.FindPropertyRelative("_valueFloatCurve").FindPropertyRelative("value");
		}
	}
}
