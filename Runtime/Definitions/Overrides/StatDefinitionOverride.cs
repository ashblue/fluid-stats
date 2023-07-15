using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    /// <summary>
    /// Used to override a pre-existing stat definition
    /// </summary>
    [System.Serializable]
    public class StatDefinitionOverride {
        [Tooltip("Definition the override will target")]
        [SerializeField]
        public StatDefinition definition;

        [Tooltip("Value of the override")]
        [SerializeField]
        public StatValueSelector value;

        public bool IsValid => definition != null && value != null;
    }
}
