namespace CleverCrow.Fluid.StatsSystem {
    [System.Serializable]
    public class StatValueSelector : StatValueSelectorBase {
        public StatValueType type;

        public override StatValueType GetValueType () {
            return type;
        }
    }
}
