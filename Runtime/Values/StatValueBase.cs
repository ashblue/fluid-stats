namespace CleverCrow.Fluid.StatsSystem {
    public abstract class StatValueBase {
        public virtual bool IsInt => false;
        public virtual bool IsFloat => false;

        public virtual int GetInt (float index) {
            return 0;
        }

        public virtual void SetInt (int value, float index = 0) {
        }

        public virtual float GetFloat (float index) {
            return 0;
        }

        public virtual void SetFloat (float value, float index = 0) {
        }
    }
}
