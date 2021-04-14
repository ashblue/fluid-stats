namespace CleverCrow.Fluid.StatsSystem {
    [System.Serializable]
    public class StatValueFloat : StatValueBase {
        public float value;

        public override bool IsFloat => true;

        public override float GetFloat (float index) {
            return value;
        }

        public override void SetFloat (float value, float index = 0) {
            this.value = value;
        }
    }
}
