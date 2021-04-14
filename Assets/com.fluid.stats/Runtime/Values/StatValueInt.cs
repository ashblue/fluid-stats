namespace CleverCrow.Fluid.StatsSystem {
    [System.Serializable]
    public class StatValueInt : StatValueBase {
        public int value;

        public override bool IsInt => true;

        public override int GetInt (float index) {
            return value;
        }

        public override void SetInt (int value, float index = 0) {
            this.value = value;
        }
    }
}
