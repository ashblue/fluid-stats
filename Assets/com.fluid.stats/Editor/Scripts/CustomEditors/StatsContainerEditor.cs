using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.StatsSystem.StatsContainers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleverCrow.Fluid.StatsSystem.Editors {
	[CustomEditor(typeof(StatsContainer))]
	public class StatsContainerEditor : Editor {
		private bool _debug;

		private StatsAdjustment _adjustment;
		private float _adjustmentIndex;
		private float _updateIndex;

		private Dictionary<StatDefinition, bool> _foldouts = new Dictionary<StatDefinition, bool>();
		private List<StatDefinition> _definitions = new List<StatDefinition>();
		private SerializedProperty _propCollection;

		private bool IsRuntimeViewable => Application.isPlaying && Target.IsSetup;

		private StatsContainer Target => (StatsContainer)target;

		private void OnEnable () {
			_propCollection = serializedObject.FindProperty("collection");

			RebuildDisplay();
		}

		public override void OnInspectorGUI () {
			_debug = EditorGUILayout.Toggle("Debug", _debug);

			EditorGUI.BeginChangeCheck();

			if (Application.isPlaying) {
				GUI.enabled = false;
			}

			serializedObject.Update();

			EditorGUILayout.PropertyField(_propCollection);

			EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);

			if (!Application.isPlaying) {
				foreach (var statDefinition in _definitions) {
					EditorGUILayout.BeginHorizontal();
					PrintInputDefault(statDefinition);
					PrintOverrideSelect(statDefinition);
					EditorGUILayout.EndHorizontal();

					PrintInputOverride(statDefinition, new GUIContent());
				}
			} else if (IsRuntimeViewable) {
				foreach (var statDefinition in _definitions) {
					PrintRuntimeStats(statDefinition);
				}
			}

			serializedObject.ApplyModifiedProperties();

			if (EditorGUI.EndChangeCheck()) {
				RebuildDisplay();
			}

			GUI.enabled = true;

			DebugUpdateAllStats();
			DebugApplyAdjustment();

			if (_debug) {
				GUI.enabled = false;
				EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
				base.OnInspectorGUI();
				GUI.enabled = true;
			}
		}

		void PrintRuntimeStats (StatDefinition definition) {
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			EditorGUILayout.BeginHorizontal();
			PrintInputOverrideOrDefault(definition);
			PrintOverrideSelect(definition);
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel++;
			var foldout = EditorGUILayout.Foldout(GetFoldout(definition), "Modifiers");

			var record = Target.GetRecord(definition);
			if (foldout && record != null) {
				EditorGUI.indentLevel++;
				PrintModifier("Add", record.modifierAdd);
				PrintModifier("Subtract", record.modifierSubtract);
				PrintModifier("Multiply", record.modifierMultiply);
				PrintModifier("Divide", record.modifierDivide);
				EditorGUI.indentLevel--;
			}

			if (record != null) {
				EditorGUILayout.FloatField("Last Retrieved Value", record.LastRetrievedValue);
			}

			EditorGUI.indentLevel--;

			EditorGUILayout.EndVertical();

			_foldouts[definition] = foldout;
		}

		bool GetFoldout (StatDefinition definition) {
			bool result;
			_foldouts.TryGetValue(definition, out result);

			return result;
		}

		void PrintModifier (string title, StatModifierCollection modifiers) {
			if (modifiers == null || modifiers.ListValues.Count == 0) return;

			EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

			foreach (var val in modifiers.ListValues) {
				EditorGUILayout.FloatField(val.id, val.value);
			}
		}

		void PrintOverrideSelect (StatDefinition definition) {
			var @override = StatValueTypeOverride.NoOverride;

			if (Target.overrides.Has(definition)) {
				@override = Target.overrides.Get(definition).value.type.ToOverride();
			}

			var oldResult = @override;
			var result = (StatValueTypeOverride)EditorGUILayout.EnumPopup(@override, GUILayout.Width(80));
			if (result == oldResult) return;

			if (result == StatValueTypeOverride.NoOverride) {
				Target.overrides.Remove(definition);
			} else if (oldResult == StatValueTypeOverride.NoOverride) {
				Target.overrides.Add(new StatDefinitionOverride {
					definition = definition,
					value = new StatValueSelector {
						type = result.ToNonOverride()
					}
				});
			} else {
				Target.overrides.Get(definition).value.type = result.ToNonOverride();
			}

			MarkDirty();
		}

		void MarkDirty () {
			EditorUtility.SetDirty(target);
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}

		void PrintInputOverrideOrDefault (StatDefinition definition) {
			if (Target.overrides.Has(definition)) {
				PrintInputOverride(definition);
			} else {
				PrintInputDefault(definition);
			}
		}

		void PrintInputOverride (StatDefinition definition, GUIContent label = null) {
			if (!Target.overrides.Has(definition)) return;

			var @override = Target.overrides.Get(definition);

			if (label == null) label = new GUIContent(definition.DisplayName, definition.Description);

			EditorGUI.BeginChangeCheck();
			PrintInputField(@override.value, label);
			if (EditorGUI.EndChangeCheck()) {
				MarkDirty();
			}
		}

		void PrintInputDefault (StatDefinition definition) {
			EditorGUI.BeginDisabledGroup(true);

			var label = new GUIContent(definition.DisplayName, definition.Description);
			PrintInputField(definition.Value, label);

			EditorGUI.EndDisabledGroup();
		}

		void PrintInputField (StatValueSelector val, GUIContent label) {
			switch (val.type) {
				case StatValueType.Int:
					var valInt = (StatValueInt)val.GetValue();
					valInt.value = EditorGUILayout.IntField(label, valInt.value);
					break;
				case StatValueType.IntCurve:
					var valIntCurve = (StatValueIntCurve)val.GetValue();
					valIntCurve.value = EditorGUILayout.CurveField(label, valIntCurve.value);
					break;
				case StatValueType.Float:
					var valFloat = (StatValueFloat)val.GetValue();
					valFloat.value = EditorGUILayout.FloatField(label, valFloat.value);
					break;
				case StatValueType.FloatCurve:
					var valFloatCurve = (StatValueFloatCurve)val.GetValue();
					valFloatCurve.value = EditorGUILayout.CurveField(label, valFloatCurve.value);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		void RebuildDisplay () {
			_definitions.Clear();

			List<StatDefinition> results;
			if (Target.collection == null) {
				results = StatDefinitionsCompiled.GetDefinitions(StatsSettings.Current.DefaultStats);
			} else {
				results = StatDefinitionsCompiled.GetDefinitions(Target.collection);
			}

			if (results == null) return;

			_definitions = results;
			_definitions = _definitions
				.OrderByDescending(d => d.SortIndex)
				.ThenBy(d => d.DisplayName)
				.ToList();

			Target.overrides.Clean();

			serializedObject.ApplyModifiedProperties();
		}

		void DebugApplyAdjustment () {
			// Never runtime debug the root parent. This will cause catastrophic data corruption
			if (!IsRuntimeViewable) return;

			EditorGUILayout.LabelField("Debug - Stats Adjustment", EditorStyles.boldLabel);

			_adjustment = (StatsAdjustment)EditorGUILayout.ObjectField("Adjustment", _adjustment, typeof(StatsAdjustment), false);
			if (_adjustment == null) return;

			_adjustmentIndex = EditorGUILayout.FloatField(new GUIContent("Adjustment Index", "Index of the applied adjustment"),
				_adjustmentIndex);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Apply")) {
				_adjustment.ApplyAdjustment(Target, _adjustmentIndex);
			}

			if (GUILayout.Button("Remove")) {
				_adjustment.RemoveAdjustment(Target);
			}
			EditorGUILayout.EndHorizontal();
		}

		void DebugUpdateAllStats () {
			// Never runtime debug the root parent. This will cause catastrophic data corruption
			if (!IsRuntimeViewable) return;

			EditorGUILayout.LabelField("Debug - Update Last Retrieved", EditorStyles.boldLabel);

			_updateIndex = EditorGUILayout.FloatField(new GUIContent("Update Index", "Index that will be applied when updating stats"),
				_updateIndex);

			if (GUILayout.Button("Update All")) {
				foreach (var r in Target.records.records) {
					if (r.Value.IsFloat) {
						Target.GetStat(r, _updateIndex);
					} else if (r.Value.IsInt) {
						Target.GetStatInt(r, _updateIndex);
					}
				}
			}
		}
	}
}
