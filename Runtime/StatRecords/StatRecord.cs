using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
	public class StatRecord {
		private bool _isDirty = true;
		private Dictionary<float, float> _valueCache = new Dictionary<float, float>();

		public StatModifierCollection modifierAdd = new StatModifierCollection(OperatorType.Add);
		public StatModifierCollection modifierSubtract = new StatModifierCollection(OperatorType.Subtract);
		public StatModifierCollection modifierMultiply = new StatModifierCollection(OperatorType.Multiply);
		public StatModifierCollection modifierDivide = new StatModifierCollection(OperatorType.Divide);

		public StatDefinition Definition { get; private set; }

		public float LastRetrievedValue { get; private set; }

		public StatValueSelector Value { get; private set; }

		public StatRecord (StatDefinition definition, StatValueSelector definitionOverride = null) {
			if (definition == null) {
				if (Application.isPlaying) {
					Debug.LogError("Cannot initialize stat record with a blank definition");
				}

				return;
			}

			Definition = definition;
			Value = definitionOverride ?? Definition.Value;

			// Initialize and setup all modifiers appropriately
			foreach (var @operator in StatsSettings.Current.OrderOfOperations.Operators) {
				var m = GetModifier(@operator.Type);
				m.onDirty += () => { _isDirty = true; };
				m.forceRound = @operator.ModifierAutoRound && definition.RoundModifiers;
			}
		}

		/// <summary>
		/// Retrieve the base value without modifiers
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public float GetBaseValue (float index = 0) {
			switch (Value.GetValueType()) {
				case StatValueType.Int:
					return GetBaseValueInt(index);
				case StatValueType.IntCurve:
					return GetBaseValueInt(index);
				case StatValueType.Float:
					return GetBaseValueFloat(index);
				case StatValueType.FloatCurve:
					return GetBaseValueFloat(index);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private int GetBaseValueInt (float index) {
			return Value.GetInt(index);
		}

		private float GetBaseValueFloat (float index) {
			return Value.GetFloat(index);
		}

		/// <summary>
		/// Retrieve the value with modifiers
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public float GetValue (float index = 0) {
			// Round the index to help with caching and retrieving float points
			var indexRound = Mathf.Round(index * 100f) / 100f;

			if (_isDirty) {
				_valueCache.Clear();
				_isDirty = false;
			} else {
                if (_valueCache.TryGetValue(indexRound, out var cacheVal)) {
					LastRetrievedValue = cacheVal;
					return cacheVal;
				}
			}

			var val = GetBaseValue(indexRound);
			foreach (var @operator in StatsSettings.Current.OrderOfOperations.Operators) {
				var o = GetModifier(@operator.Type);
				val = o.ModifyValue(val);
			}

			if (Definition.RoundResult) {
				val = Mathf.Round(val);
			}

			_valueCache[indexRound] = val;
			LastRetrievedValue = val;

			return val;
		}

		/// <summary>
		/// Retrieve the modifier collection by operator
		/// </summary>
		/// <param name="operator"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public StatModifierCollection GetModifier (OperatorType @operator) {
			switch (@operator) {
				case OperatorType.Add:
					return modifierAdd;
				case OperatorType.Subtract:
					return modifierSubtract;
				case OperatorType.Multiply:
					return modifierMultiply;
				case OperatorType.Divide:
					return modifierDivide;
				default:
					throw new ArgumentOutOfRangeException("operator", @operator, null);
			}
		}
	}
}

