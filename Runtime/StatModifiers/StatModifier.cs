namespace CleverCrow.Fluid.StatsSystem {
    public class StatModifier {
        public readonly string id;
        public float value;

        public StatModifier (string id, float value) {
            this.id = id;
            this.value = value;
        }
    }
}
