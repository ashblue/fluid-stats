using System.Collections.Generic;
using CleverCrow.Fluid.StatsSystem.StatsContainers;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors.Testing {
    public class TestStatsData : TestSettingsBase {
        private StatDefinitionCollection _statCol;
        private StatDefinitionCollection _statColDefault;
        private StatDefinition _dodge;
        private StatDefinition _health;
        private StatDefinition _attack;
        private StatDefinition _armor;
        private StatsContainer _data;

        [SetUp]
        public void SetupStatsData () {
            _statColDefault = ScriptableObject.CreateInstance<StatDefinitionCollection>();
            SetPrivateField(settings, "_defaultStats", _statColDefault);

            _statCol = ScriptableObject.CreateInstance<StatDefinitionCollection>();

            _health = ScriptableObject.CreateInstance<StatDefinition>();
            SetPrivateField(_health, "_id", "health");
            _health.Value.type = StatValueType.Int;
            _health.Value.GetValue().SetInt(10);
            _statColDefault.definitions.Add(_health);

            _attack = ScriptableObject.CreateInstance<StatDefinition>();
            SetPrivateField(_attack, "_id", "attack");
            _attack.Value.type = StatValueType.Float;
            _attack.Value.GetValue().SetFloat(3);
            _statCol.definitions.Add(_attack);

            _armor = ScriptableObject.CreateInstance<StatDefinition>();
            SetPrivateField(_armor, "_id", "armor");
            _armor.Value.type = StatValueType.FloatCurve;
            var armorValue = _armor.Value.GetValue();
            armorValue.SetFloat(2, 2);
            armorValue.SetFloat(3, 3);
            _statCol.definitions.Add(_armor);

            _dodge = ScriptableObject.CreateInstance<StatDefinition>();
            SetPrivateField(_dodge, "_id", "_dodge");
            _dodge.Value.type = StatValueType.IntCurve;
            var dodgeValue = _dodge.Value.GetValue();
            dodgeValue.SetInt(2, 2);
            dodgeValue.SetInt(3, 3);
            _statCol.definitions.Add(_dodge);

            _data = ScriptableObject.CreateInstance<StatsContainer>();
            _data.collection = _statCol;
        }

        [Test]
        public void NoDefaultCollectionDoesNotCrashOnSetup () {
            SetPrivateField<StatsSettings, StatDefinitionCollection>(settings, "_defaultStats", null);
            _data.collection = null;
            _data.Setup();
        }

        [Test]
        public void NoCollectionPopulatesDefaultCollection () {
            _data.collection = null;
            _data.Setup();

            Assert.IsTrue(_data.HasRecord(_health));
            Assert.IsFalse(_data.HasRecord(_armor));
        }

        [Test]
        public void RecordsPopulatedOnProperSetup () {
            _data.Setup();

            foreach (StatDefinition def in _data.collection.definitions) {
                Assert.IsTrue(_data.HasRecord(def));
            }
        }

        [Test]
        public void OverridesHijackRecordValues () {
            var overrideHealth = new StatDefinitionOverride {
                definition = _health,
                value = new StatValueSelector { type = StatValueType.Float }
            };
            overrideHealth.value.GetValue().SetFloat(5);
            _data.overrides.Add(overrideHealth);

            _data.Setup();

            Assert.AreEqual(5f, _data.GetStat(_health));
        }

        [Test]
        public void GetRecordByIdNullOrEmptyString () {
            _data.Setup();

            Assert.IsNull(_data.GetRecord(""));

            var nullString = "";
            Assert.IsNull(_data.GetRecord(nullString));
        }

        [Test]
        public void GetRecordById () {
            _data.Setup();

            Assert.IsNotNull(_data.GetRecord("health"));
        }

        [Test]
        public void GetRecordNull () {
            _data.Setup();

            StatDefinition nullDef = null;
            Assert.IsNull(_data.GetRecord(nullDef));
        }

        [Test]
        public void GetRecord () {
            _data.Setup();

            Assert.IsNotNull(_data.GetRecord(_health));
        }

        [Test]
        public void HasRecordByIdNullOrEmptyString () {
            _data.Setup();

            Assert.IsFalse(_data.HasRecord(""));

            var nullString = "";
            Assert.IsFalse(_data.HasRecord(nullString));
        }

        [Test]
        public void HasRecordById () {
            _data.Setup();

            Assert.IsTrue(_data.HasRecord("health"));
        }

        [Test]
        public void HasRecordNull () {
            _data.Setup();

            StatDefinition nullDef = null;
            Assert.IsFalse(_data.HasRecord(nullDef));
        }

        [Test]
        public void HasRecord () {
            _data.Setup();

            Assert.IsTrue(_data.HasRecord(_health));
        }

        [Test]
        public void GetStatNull () {
            _data.Setup();

            var nullString = "";
            Assert.AreEqual(0, _data.GetStat(nullString));
        }

        [Test]
        public void GetStat () {
            _data.Setup();

            Assert.AreEqual(3, _data.GetStat(_attack));
        }

        [Test]
        public void GetStatCurve () {
            _data.Setup();

            Assert.AreEqual(3, _data.GetStat(_armor, 3));
        }

        [Test]
        public void GetStatIntNull () {
            _data.Setup();

            var nullString = "";
            Assert.AreEqual(0, _data.GetStatInt(nullString));
        }

        [Test]
        public void GetStatInt () {
            _data.Setup();

            Assert.AreEqual(10, _data.GetStatInt(_health));
        }

        [Test]
        public void GetStatIntCurve () {
            _data.Setup();

            Assert.AreEqual(3, _data.GetStatInt(_armor, 3));
        }

        [Test]
        public void SetModifierDefinitionNullOrEmpty () {
            _data.Setup();

            StatDefinition def = null;
            _data.SetModifier(OperatorType.Add, def, "asdf", 2);
            _data.SetModifier(OperatorType.Add, "", "asdf", 2);
        }

        [Test]
        public void SetModifierIdNullOrEmpty () {
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _health, null, 2);
            _data.SetModifier(OperatorType.Add, _health, "", 2);

            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _health, ""));
        }

        [Test]
        public void SetModifier () {
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _health, "asdf", 2);

            Assert.AreEqual(2, _data.GetModifier(OperatorType.Add, _health, "asdf"));
        }

        [Test]
        public void GetModiferNullOrEmpty () {
            _data.Setup();

            StatDefinition def = null;
            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, def, "asdf"));
            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, "", "asdf"));
        }

        [Test]
        public void GetModifierIdNullOrEmpty () {
            _data.Setup();

            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _health, null));
            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _health, ""));
        }

        [Test]
        public void GetModifier () {
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _health, "asdf", 2);

            Assert.AreEqual(2, _data.GetModifier(OperatorType.Add, _health, "asdf"));
        }

        [Test]
        public void GetModifierWhenModifierDoesNotExist () {
            _data.Setup();

            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _health, "asdf"));
        }

        [Test]
        public void RemoveModiferNullOrEmpty () {
            _data.Setup();

            StatDefinition def = null;
            Assert.IsFalse(_data.RemoveModifier(OperatorType.Add, def, "asdf"));
            Assert.IsFalse(_data.RemoveModifier(OperatorType.Add, "", "asdf"));
        }

        [Test]
        public void RemoveModifierIdNullOrEmpty () {
            _data.Setup();

            Assert.IsFalse(_data.RemoveModifier(OperatorType.Add, _health, null));
            Assert.IsFalse(_data.RemoveModifier(OperatorType.Add, _health, ""));
        }

        [Test]
        public void RemoveModifier () {
            _data.Setup();

            _data.SetModifier(OperatorType.Add, _health, "asdf", 2);

            Assert.IsTrue(_data.RemoveModifier(OperatorType.Add, _health, "asdf"));
            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _health, "asdf"));
        }

        [Test]
        public void RemoveModifierWhenModifierDoesNotExist () {
            _data.Setup();

            Assert.IsFalse(_data.RemoveModifier(OperatorType.Add, _health, "asdf"));
        }

        [Test]
        public void ClearAllModifiersNullList () {
            List<string> nullList = null;
            _data.ClearAllModifiers(nullList);
        }

        [Test]
        public void ClearAllModifiersByList () {
            _data.SetModifier(OperatorType.Add, _health, "asdf", 2);
            _data.SetModifier(OperatorType.Add, _armor, "asdf", 2);

            var list = new List<string> { "health", "armor" };
            _data.ClearAllModifiers(list);

            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _health, "asdf"));
            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _armor, "asdf"));
        }

        [Test]
        public void ClearAllModifiersById () {
            _data.SetModifier(OperatorType.Add, _health, "asdf", 2);
            _data.SetModifier(OperatorType.Add, _armor, "asdf", 2);

            _data.ClearAllModifiers("asdf");

            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _health, "asdf"));
            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _armor, "asdf"));
        }

        [Test]
        public void ClearAllModifiersByRecordAndModifier () {
            _data.SetModifier(OperatorType.Add, _health, "asdf", 2);

            _data.ClearAllModifiers(_data.GetRecord(_health), "asdf");

            Assert.AreEqual(0, _data.GetModifier(OperatorType.Add, _health, "asdf"));
        }
    }
}
