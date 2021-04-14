using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    [System.Serializable]
    public class Operator {
        [Tooltip("Type of operations")]
        [SerializeField]
        private OperatorType _type;

        [Tooltip("Automatically round this operation as a modifier if `forceRound` is enabled on the definition")]
        [SerializeField]
        private bool _modifierAutoRound;

        public OperatorType Type => _type;

        public bool ModifierAutoRound => _modifierAutoRound;

        public Operator () {
            // Unity serialized initializer
        }

        public Operator (OperatorType type, bool modifierAutoRound) {
            _type = type;
            _modifierAutoRound = modifierAutoRound;
        }
    }
}
