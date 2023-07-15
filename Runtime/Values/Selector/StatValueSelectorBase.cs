using System;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    public abstract class StatValueSelectorBase {
        public bool IsFloat {
            get { return GetValue().IsFloat; }
        }

        public bool IsInt {
            get { return GetValue().IsInt; }
        }

        [SerializeField]
        protected StatValueInt _valueInt = new StatValueInt();

        [SerializeField]
        protected StatValueIntCurve _valueIntCurve = new StatValueIntCurve();

        [SerializeField]
        protected StatValueFloat _valueFloat = new StatValueFloat();

        [SerializeField]
        protected StatValueFloatCurve _valueFloatCurve = new StatValueFloatCurve();

        public int GetInt (float index) {
            return GetValue().GetInt(index);
        }

        public float GetFloat (float index) {
            return GetValue().GetFloat(index);
        }

        public abstract StatValueType GetValueType ();

        public StatValueBase GetValue () {
            var valueType = GetValueType();
            switch (valueType) {
                case StatValueType.Int:
                    return _valueInt;
                case StatValueType.IntCurve:
                    return _valueIntCurve;
                case StatValueType.Float:
                    return _valueFloat;
                case StatValueType.FloatCurve:
                    return _valueFloatCurve;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
