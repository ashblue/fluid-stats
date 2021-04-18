using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    [System.Serializable]
    public class StatValueIntCurve : StatValueBase {
        public AnimationCurve value = new AnimationCurve();

        public override bool IsInt => true;

        public override int GetInt (float index) {
            return Mathf.RoundToInt(value.Evaluate(index));
        }

        public override void SetInt (int value, float index = 0) {
            var keyIndex = this.value.keys.ToList().FindIndex(k => Mathf.Abs(k.time - index) < 0.1f);
            if (keyIndex != -1) this.value.RemoveKey(keyIndex);

            this.value.AddKey(index, value);
        }
    }
}
