using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    [System.Serializable]
    public class StatValueFloatCurve : StatValueBase {
        public AnimationCurve value = new AnimationCurve();

        public override bool IsFloat => true;

        public override float GetFloat (float index) {
            return value.Evaluate(index);
        }

        public override void SetFloat (float value, float index = 0) {
            var keyIndex = this.value.keys.ToList().FindIndex(k => Mathf.Abs(k.time - index) < 0.1f);
            if (keyIndex != -1) this.value.RemoveKey(keyIndex);

            this.value.AddKey(index, value);
        }
    }
}
