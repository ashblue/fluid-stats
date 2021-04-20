namespace CleverCrow.Fluid.StatsSystem {
    [System.Serializable]
    public class ModifierGroup {
        public StatDefinition definition;
        public StatValueSelector value = new StatValueSelector();
        public OperatorType operatorType;

        public bool IsValid => definition != null && value != null;

        public float GetValue (float index) {
            if (!IsValid) return 0;

            if (value.IsFloat) {
                return value.GetFloat(index);
            }

            if (value.IsInt) {
                return value.GetInt(index);
            }

            return 0;
        }
    }
}
