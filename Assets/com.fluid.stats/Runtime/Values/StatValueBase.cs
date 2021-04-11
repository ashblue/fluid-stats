namespace Adnc.StatsSystem {
    public abstract class StatValueBase {        
        public virtual bool IsInt { get { return false; } }
        public virtual bool IsFloat { get { return false; } }

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
