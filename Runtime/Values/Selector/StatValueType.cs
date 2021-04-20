namespace CleverCrow.Fluid.StatsSystem {
    public enum StatValueType {
        Int,
        IntCurve,
        Float,
        FloatCurve
    }

    public static class StatValueTypeMethods {
        public static StatValueTypeOverride ToOverride (this StatValueType type) {
            var i = (int)type;
            i += 1;

            return (StatValueTypeOverride)i;
        }
    }
}
