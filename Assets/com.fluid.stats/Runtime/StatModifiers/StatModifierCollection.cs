using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    public class StatModifierCollection {
        private OperatorType _type;

        private Dictionary<string, StatModifier> _dicValues = new Dictionary<string, StatModifier>();
        private List<StatModifier> _listValues = new List<StatModifier>();

        /// <summary>
        /// Round all set numbers automatically
        /// </summary>
        public bool forceRound;

        public delegate void Action ();
        public event Action onDirty;

        /// <summary>
        /// List of all available values. WARNING editing this list directly will cause the StatModifierGroup to crash.
        /// </summary>
        public List<StatModifier> ListValues => _listValues;

        public StatModifierCollection (OperatorType type) {
            _type = type;
        }

        /// <summary>
        /// Retrieve the modifier by id
        /// </summary>
        /// <param name="id">Identifier.</param>
        public StatModifier Get (string id) {
            if (string.IsNullOrEmpty(id) || !_dicValues.TryGetValue(id, out var output)) {
                return null;
            }

            return output;
        }

        /// <summary>
        /// Set or change a modifier
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="value">Value.</param>
        public void Set (string id, float value) {
            if (string.IsNullOrEmpty(id)) {
                return;
            }

            if (forceRound) {
                value = Mathf.Round(value);
            }

            if (_dicValues.TryGetValue(id, out var mod)) {
                mod.value = value;
            } else {
                // No value was found to modify, create a new one
                mod = new StatModifier(id, value);

                _dicValues[id] = mod;
                _listValues.Add(mod);
            }

            if (onDirty != null) onDirty.Invoke();
        }

        /// <summary>
        /// Wipes a modifier from memory
        /// </summary>
        /// <param name="id">Identifier.</param>
        public bool Remove (string id) {
            var target = Get(id);

            if (target == null) {
                return false;
            }

            _dicValues.Remove(id);
            _listValues.Remove(target);

            if (onDirty != null) onDirty.Invoke();

            return true;
        }

        /// <summary>
        /// Modify a value based upon the modifiers
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public float ModifyValue (float original) {
            if (_type == OperatorType.Multiply) {
                var multiplier = 0f;
                foreach (var statModifier in _listValues) {
                    multiplier += statModifier.value;
                }

                return Mathf.Max(0, original + original * multiplier);
            }

            var newVal = original;
            foreach (var statModifier in _listValues) {
                switch (_type) {
                    case OperatorType.Add:
                        newVal += statModifier.value;
                        break;
                    case OperatorType.Subtract:
                        newVal -= statModifier.value;
                        break;
                    case OperatorType.Divide:
                        newVal /= statModifier.value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return newVal;
        }
    }
}
