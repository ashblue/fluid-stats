using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    public enum StatValueTypeOverride {
        NoOverride,
        Int,
        IntCurve,
        Float,
        FloatCurve
    }

    public static class StatValueTypeOverrideMethods {
        public static StatValueType ToNonOverride (this StatValueTypeOverride type) {
            var i = (int)type;
            i -= 1;
            i = Mathf.Max(0, i);

            return (StatValueType)i;
        }
    }
}
