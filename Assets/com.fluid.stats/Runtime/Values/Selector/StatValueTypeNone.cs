using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    public enum StatValueTypeNone {
        None,
        Int,
        IntCurve,
        Float,
        FloatCurve
    }

    public static class StatValueTypeNoneMethods {
        public static StatValueType ToStatValueType (this StatValueTypeNone type) {
            var i = (int)type;
            i -= 1;
            i = Mathf.Max(0, i);

            return (StatValueType)i;
        }
    }
}
