using CleverCrow.Fluid.StatsSystem.StatsContainers;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors.Testing {
    public class TestStatsAdjustment : TestSettingsBase {
        private StatDefinitionCollection _statCol;
        private StatDefinition _health;
        private StatDefinition _attack;

        private StatsContainer _data;

        private StatsAdjustment _adjustment;

        [SetUp]
        public void SetupStatsAdjustment () {
            _adjustment = ScriptableObject.CreateInstance<StatsAdjustment>();
            _statCol = ScriptableObject.CreateInstance<StatDefinitionCollection>();

            _health = ScriptableObject.CreateInstance<StatDefinition>();
            SetPrivateField(_health, "_id", "health");
            _health.Value.type = StatValueType.Int;
            _health.Value.GetValue().SetInt(10);
            _statCol.definitions.Add(_health);

            _attack = ScriptableObject.CreateInstance<StatDefinition>();
            SetPrivateField(_attack, "_id", "attack");
            _attack.Value.type = StatValueType.Float;
            _attack.Value.GetValue().SetFloat(3);
            _statCol.definitions.Add(_attack);

            _data = ScriptableObject.CreateInstance<StatsContainer>();
            _data.collection = _statCol;
        }

        [Test]
        public void RandomIdGeneratedIfNoIdSet () {
            Assert.IsNotNull(_adjustment.Id);
            Assert.AreNotEqual("", _adjustment.Id);
        }

        [Test]
        public void IdSetReturnsSetValue () {
            SetPrivateField(_adjustment, "_id", "asdf");

            Assert.AreEqual("asdf", _adjustment.Id);
        }

        [Test]
        public void ApplyAdjustment () {
            _data.Setup();

            var m = new ModifierGroup {
                definition = _health,
                operatorType = OperatorType.Add,
                value = new StatValueSelector { type = StatValueType.Int }
            };

            m.value.GetValue().SetInt(3);

            _adjustment.Adjustments.Add(m);

            _adjustment.ApplyAdjustment(_data, 0);

            Assert.AreEqual(13, _data.GetStatInt(_health));
        }

        [Test]
        public void ApplyAdjustmentNull () {
            _data.Setup();

            _adjustment.Adjustments.Add(null);

            _adjustment.ApplyAdjustment(_data, 0);

            Assert.AreEqual(10, _data.GetStatInt(_health));
        }

        [Test]
        public void ApplyAdjustmentNullGroupModifierDefinitionsDoNotCrash () {
            _data.Setup();

            var m = new ModifierGroup {
                definition = null,
                operatorType = OperatorType.Add,
                value = new StatValueSelector { type = StatValueType.Int }
            };

            _adjustment.Adjustments.Add(m);

            _adjustment.ApplyAdjustment(_data, 0);

            Assert.AreEqual(10, _data.GetStatInt(_health));
        }

        [Test]
        public void RemoveAdjustment () {
            _data.Setup();

            var m = new ModifierGroup {
                definition = _health,
                operatorType = OperatorType.Add,
                value = new StatValueSelector { type = StatValueType.Int }
            };

            m.value.GetValue().SetInt(3);

            _adjustment.Adjustments.Add(m);

            _adjustment.ApplyAdjustment(_data, 0);
            _adjustment.RemoveAdjustment(_data);

            Assert.AreEqual(10, _data.GetStatInt(_health));
        }

        [Test]
        public void RemoveAdjustmentWithNoRemoval () {
            _data.Setup();

            var m = new ModifierGroup {
                definition = _health,
                operatorType = OperatorType.Add,
                value = new StatValueSelector { type = StatValueType.Int }
            };

            m.value.GetValue().SetInt(3);

            _adjustment.Adjustments.Add(m);

            _adjustment.RemoveAdjustment(_data);

            Assert.AreEqual(10, _data.GetStatInt(_health));
        }

        [Test]
        public void RemoveAdjustmentNull () {
            _data.Setup();

            _adjustment.Adjustments.Add(null);

            _adjustment.RemoveAdjustment(_data);

            Assert.AreEqual(10, _data.GetStatInt(_health));
        }

        [Test]
        public void RemoveAdjustmentNullGroupModifierDefinitionsDoNotCrash () {
            _data.Setup();

            var m = new ModifierGroup {
                definition = null,
                operatorType = OperatorType.Add,
                value = new StatValueSelector { type = StatValueType.Int }
            };

            m.value.GetValue().SetInt(3);

            _adjustment.Adjustments.Add(m);

            _adjustment.ApplyAdjustment(_data, 0);
            _adjustment.RemoveAdjustment(_data);

            Assert.AreEqual(10, _data.GetStatInt(_health));
        }
    }
}
