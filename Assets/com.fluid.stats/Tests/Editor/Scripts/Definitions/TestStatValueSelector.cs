using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors.Testing {
    public class TestStatValueSelector : TestBase {
        public static StatDefinition CreateDefinition (StatValueType type, float value) {
            var d = ScriptableObject.CreateInstance<StatDefinition>();
            d.Value.type = type;

            switch (type) {
                case StatValueType.Int:
                    var vInt = (StatValueInt)d.Value.GetValue();
                    vInt.value = Mathf.RoundToInt(value);
                    break;
                case StatValueType.IntCurve:
                    var vIntCurve = (StatValueIntCurve)d.Value.GetValue();
                    vIntCurve.value = new AnimationCurve(new Keyframe(0, value));
                    break;
                case StatValueType.Float:
                    var vFloat = (StatValueFloat)d.Value.GetValue();
                    vFloat.value = value;
                    break;
                case StatValueType.FloatCurve:
                    var vFloatCurve = (StatValueFloatCurve)d.Value.GetValue();
                    vFloatCurve.value = new AnimationCurve(new Keyframe(0, value));
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException("type", type, null);
            }

            return d;
        }

        [Test]
        public void GetValueInt () {
            var d = CreateDefinition(StatValueType.Int, 7);
            var v = d.Value.GetInt(0);

            Assert.AreEqual(7, v);
            Assert.AreEqual(0, d.Value.GetFloat(0));
        }

        [Test]
        public void GetValueIntCurve () {
            var d = CreateDefinition(StatValueType.IntCurve, 5);
            var v = d.Value.GetInt(0);

            Assert.AreEqual(5, v);
            Assert.AreEqual(0, d.Value.GetFloat(0));
        }

        [Test]
        public void GetValueFloat () {
            var d = CreateDefinition(StatValueType.Float, 9.5f);
            var v = d.Value.GetFloat(0);

            Assert.AreEqual(9.5f, v);
            Assert.AreEqual(0, d.Value.GetInt(0));
        }

        [Test]
        public void GetValueFloatCurve () {
            var d = CreateDefinition(StatValueType.FloatCurve, 9.5f);
            var v = d.Value.GetFloat(0);

            Assert.AreEqual(9.5f, v);
            Assert.AreEqual(0, d.Value.GetInt(0));
        }
    }
}
