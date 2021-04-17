using System.Collections.Generic;
using CleverCrow.Fluid.StatsSystem.StatsContainers;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    [CreateAssetMenu(fileName = "StatsAdjustment", menuName = "Fluid/Stats/Adjustment")]
    public class StatsAdjustment : ScriptableObject {
        // Storage container for lazy loaded GUIDs
        private string _randomId;

        [Tooltip("ID used to add and remove stats. Failure to provide an ID will create a random GUID")]
        [SerializeField]
        private string _id;

        [Tooltip("Apply this operator to all adjustments")]
        [SerializeField]
        private OperatorTypeNone _forceOperator;

        [Tooltip("Apply this display type to all adjustments")]
        [SerializeField]
        private StatValueTypeNone _forceDisplay;

        [Tooltip("Available adjustments")]
        [SerializeField]
        private List<ModifierGroup> _adjustments = new List<ModifierGroup>();

        public List<ModifierGroup> Adjustments => _adjustments;

        /// <summary>
        /// String ID or randomly generated GUID if no ID is present
        /// </summary>
        public string Id {
            get {
                if (string.IsNullOrEmpty(_id)) {
                    if (string.IsNullOrEmpty(_randomId)) _randomId = System.Guid.NewGuid().ToString();
                    return _randomId;
                }

                return _id;
            }
        }


        /// <summary>
        /// Apply all modifiers to the StatsData target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        public void ApplyAdjustment (StatsContainer target, float index = 0) {
            foreach (var adj in _adjustments) {
                if (adj == null || !adj.IsValid) continue;
                target.SetModifier(adj.operatorType, adj.definition, Id, adj.GetValue(index));
            }
        }

        /// <summary>
        /// Remove all modifiers from the StatsData target
        /// </summary>
        /// <param name="target"></param>
        public void RemoveAdjustment (StatsContainer target) {
            foreach (var adj in _adjustments) {
                if (adj == null || !adj.IsValid) continue;
                target.RemoveModifier(adj.operatorType, adj.definition, Id);
            }
        }
    }
}
