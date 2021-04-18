using CleverCrow.Fluid.StatsSystem.StatsContainers;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors.Testing {
    public class TestStatsDataRoundedValues : TestSettingsBase {
        private const float MULT = 2.2f;
        private const float ADD = 7.7f;

        private const float FLOAT_BASE = 0.6f;
        private const int INT_BASE = 1;

        private StatsContainer _data;
        private StatDefinitionCollection _statCol;
        private StatDefinition _statInt;
        private StatDefinition _statFloat;

        [SetUp]
        public void SetupStatsData () {
            _statCol = ScriptableObject.CreateInstance<StatDefinitionCollection>();

            _statInt = ScriptableObject.CreateInstance<StatDefinition>();
            _statInt.Value.type = StatValueType.Int;
            _statCol.definitions.Add(_statInt);
            _statInt.Value.GetValue().SetInt(INT_BASE);

            _statFloat = ScriptableObject.CreateInstance<StatDefinition>();
            _statFloat.Value.type = StatValueType.Float;
            _statCol.definitions.Add(_statFloat);
            _statFloat.Value.GetValue().SetFloat(FLOAT_BASE);

            _data = ScriptableObject.CreateInstance<StatsContainer>();
            _data.collection = _statCol;
        }

        [Test]
        public void RoundedInt () {
            SetPrivateField(_statInt, "_roundModifiers", false);
            SetPrivateField(_statInt, "_roundResult", true);
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _statInt, "add", ADD);
            _data.SetModifier(OperatorType.Multiply, _statInt, "mult", MULT);
            var result = INT_BASE + ADD;
            result += result * MULT;

            Assert.AreEqual(Mathf.RoundToInt(result), _data.GetStatInt(_statInt));
        }

        [Test]
        public void RoundedIntModifiers () {
            SetPrivateField(_statInt, "_roundModifiers", true);
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _statInt, "add", ADD);
            _data.SetModifier(OperatorType.Multiply, _statInt, "mult", MULT);
            var result = INT_BASE + Mathf.Round(ADD);
            result += result * MULT;

            Assert.AreEqual(Mathf.RoundToInt(result), _data.GetStatInt(_statInt));
        }

        [Test]
        public void NotRoundedInt () {
            SetPrivateField(_statInt, "_roundModifiers", false);
            SetPrivateField(_statInt, "_roundResult", false);
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _statInt, "add", ADD);
            _data.SetModifier(OperatorType.Multiply, _statInt, "mult", MULT);
            var result = INT_BASE + ADD;
            result += result * MULT;

            Assert.AreEqual(Mathf.RoundToInt(result), _data.GetStatInt(_statInt));
        }

        [Test]
        public void RoundedFloat () {
            SetPrivateField(_statFloat, "_roundModifiers", false);
            SetPrivateField(_statFloat, "_roundResult", true);
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _statFloat, "add", ADD);
            _data.SetModifier(OperatorType.Multiply, _statFloat, "mult", MULT);
            var result = FLOAT_BASE + ADD;
            result += result * MULT;

            Assert.AreEqual(Mathf.Round(result), _data.GetStat(_statFloat));
        }

        [Test]
        public void RoundedFloatModifiers () {
            SetPrivateField(_statFloat, "_roundModifiers", true);
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _statFloat, "add", ADD);
            _data.SetModifier(OperatorType.Multiply, _statFloat, "mult", MULT);
            var result = FLOAT_BASE + Mathf.Round(ADD);
            result += result * MULT;

            var difference = Mathf.Abs(result - _data.GetStat(_statFloat));

            Assert.IsTrue(difference < 0.01);
        }

        [Test]
        public void NotRoundedFloat () {
            SetPrivateField(_statFloat, "_roundModifiers", false);
            SetPrivateField(_statFloat, "_roundResult", false);
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _statFloat, "add", ADD);
            _data.SetModifier(OperatorType.Multiply, _statFloat, "mult", MULT);
            var result = FLOAT_BASE + ADD;
            result += result * MULT;

            Assert.AreEqual(result, _data.GetStat(_statFloat));
        }
    }
}
