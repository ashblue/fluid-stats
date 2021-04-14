using CleverCrow.Fluid.StatsSystem.StatsContainers;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CleverCrow.Fluid.StatsSystem.Editors.Testing {
    public class StatsContainerTest : TestSettingsBase {
        private StatDefinitionCollection _statCol;
        private StatDefinitionCollection _statColDefault;
        private StatDefinition _dodge;
        private StatDefinition _health;
        private StatDefinition _attack;
        private StatDefinition _armor;
        private StatsContainer _container;

        [SetUp]
        public void BeforeEach () {
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

            _container = ScriptableObject.CreateInstance<StatsContainer>();
            _container.collection = _statCol;
        }

        public class OnStatChangeSubscribeMethod : StatsContainerTest {
            [Test]
            public void It_should_return_the_new_value_on_SetModifier () {
                _health.Value.GetValue().SetInt(0);

                StatRecord result = null;
                var copy = _container.CreateRuntimeCopy();
                copy.OnStatChangeSubscribe("health", (record) => result = record);
                copy.SetModifier(OperatorType.Add, "health", "plus", 5f);

                Assert.AreEqual(5f, result.GetValue());
            }

            [Test]
            public void It_should_support_multiple_event_subscriptions_for_the_same_stat () {
                var result = 0;
                var copy = _container.CreateRuntimeCopy();
                copy.OnStatChangeSubscribe("health", (record) => result += 1);
                copy.OnStatChangeSubscribe("health", (record) => result += 1);
                copy.SetModifier(OperatorType.Add, "health", "plus", 5f);

                Assert.AreEqual(2, result);
            }

            [Test]
            public void It_should_not_fire_if_event_is_not_triggered () {
                var result = 0;
                var copy = _container.CreateRuntimeCopy();
                copy.OnStatChangeSubscribe("health", (record) => result += 1);

                Assert.AreEqual(0, result);
            }

            [Test]
            public void It_should_trigger_an_event_on_RemoveModifier () {
                _health.Value.GetValue().SetInt(0);

                StatRecord result = null;
                var copy = _container.CreateRuntimeCopy();
                copy.SetModifier(OperatorType.Add, "health", "plus", 5f);
                copy.OnStatChangeSubscribe("health", (record) => result = record);
                copy.RemoveModifier(OperatorType.Add, "health", "plus");

                Assert.AreEqual(0f, result.GetValue());
            }

            [Test]
            public void It_should_trigger_an_event_on_ClearAllModifiers () {
                _health.Value.GetValue().SetInt(0);

                StatRecord result = null;
                var copy = _container.CreateRuntimeCopy();
                copy.SetModifier(OperatorType.Add, "health", "plus", 5f);
                copy.OnStatChangeSubscribe("health", (record) => result = record);
                copy.ClearAllModifiers("plus");

                Assert.AreEqual(0f, result.GetValue());
            }

            [Test]
            public void It_should_subscribe_with_a_definition () {
                _health.Value.GetValue().SetInt(0);

                StatRecord result = null;
                var copy = _container.CreateRuntimeCopy();
                copy.OnStatChangeSubscribe(_health, (record) => result = record);
                copy.SetModifier(OperatorType.Add, "health", "plus", 5f);

                Assert.AreEqual(5f, result.GetValue());
            }

            [Test]
            public void It_should_subscribe_with_a_record () {
                _health.Value.GetValue().SetInt(0);

                StatRecord result = null;
                var copy = _container.CreateRuntimeCopy();
                copy.OnStatChangeSubscribe(copy.GetRecord("health"), (record) => result = record);
                copy.SetModifier(OperatorType.Add, "health", "plus", 5f);

                Assert.AreEqual(5f, result.GetValue());
            }
        }

        public class OnStatChangeUnsubscribe : StatsContainerTest {
            [Test]
            public void It_should_remove_the_passed_subscription () {
                _health.Value.GetValue().SetInt(0);

                StatRecord result = null;
                var copy = _container.CreateRuntimeCopy();
                var record = copy.GetRecord("health");
                var action = new UnityAction<StatRecord>(r => result = r);
                copy.OnStatChangeSubscribe(record, action);
                copy.OnStatChangeUnsubscribe(record, action);
                copy.SetModifier(OperatorType.Add, "health", "plus", 5f);

                Assert.IsNull(result);
            }

            [Test]
            public void It_should_remove_the_passed_subscription_via_definition () {
                _health.Value.GetValue().SetInt(0);

                StatRecord result = null;
                var copy = _container.CreateRuntimeCopy();
                var record = copy.GetRecord("health");
                var action = new UnityAction<StatRecord>(r => result = r);
                copy.OnStatChangeSubscribe(record, action);
                copy.OnStatChangeUnsubscribe(_health, action);
                copy.SetModifier(OperatorType.Add, "health", "plus", 5f);

                Assert.IsNull(result);
            }

            [Test]
            public void It_should_remove_the_passed_subscription_via_string () {
                _health.Value.GetValue().SetInt(0);

                StatRecord result = null;
                var copy = _container.CreateRuntimeCopy();
                var record = copy.GetRecord("health");
                var action = new UnityAction<StatRecord>(r => result = r);
                copy.OnStatChangeSubscribe(record, action);
                copy.OnStatChangeUnsubscribe("health", action);
                copy.SetModifier(OperatorType.Add, "health", "plus", 5f);

                Assert.IsNull(result);
            }
        }

        public class SetupMethod : StatsContainerTest {
            [Test]
            public void By_default_IsSetup_is_false () {
                Assert.IsFalse(_container.IsSetup);
            }

            [Test]
            public void Causes_IsSetup_to_be_marked_true () {
                _container.Setup();
                Assert.IsTrue(_container.IsSetup);
            }

            [Test]
            public void Populates_default_records_on_setup () {
                _container.Setup();

                foreach (var statDefinitionBase in _container.collection.definitions) {
                    var def = (StatDefinition)statDefinitionBase;
                    Assert.IsTrue(_container.HasRecord(def));
                }
            }
        }

        public class DuplicateRecords : StatsContainerTest {
            [Test]
            public void Changing_one_record_modifier_does_not_affect_another () {
                _container.Setup();
                _container.SetModifier(OperatorType.Add, _health, "asdf", 10);
                var val = _container.GetStatInt(_health);

                var containerAlt = ScriptableObject.CreateInstance<StatsContainer>();
                containerAlt.collection = _statCol;
                containerAlt.Setup();

                Assert.AreNotEqual(val, containerAlt.GetStatInt(_health));
            }
        }

        public class CreateRuntimeCopyMethod : StatsContainerTest {
            [Test]
            public void It_should_return_a_copy () {
                var copy = _container.CreateRuntimeCopy();

                Assert.IsNotNull(copy);
            }

            [Test]
            public void It_should_run_setup_on_the_copy () {
                var copy = _container.CreateRuntimeCopy();

                Assert.IsTrue(copy.IsSetup);
            }

            [Test]
            public void It_should_copy_over_records () {
                var copy = _container.CreateRuntimeCopy();

                Assert.IsTrue(copy.HasRecord(_health));
            }
        }

        public class ArchiveTest : StatsContainerTest {
            [Test]
            public void OverridesHijackRecordValues () {
                var overrideHealth = new StatDefinitionOverride {
                    definition = _health,
                    value = new StatValueSelector {type = StatValueType.Float}
                };
                overrideHealth.value.GetValue().SetFloat(5);
                _container.overrides.Add(overrideHealth);

                _container.Setup();

                Assert.AreEqual(5f, _container.GetStat(_health));
            }

            [Test]
            public void GetRecordByIdNullOrEmptyString () {
                _container.Setup();

                Assert.IsNull(_container.GetRecord(""));

                var nullString = "";
                Assert.IsNull(_container.GetRecord(nullString));
            }

            [Test]
            public void GetRecordById () {
                _container.Setup();

                Assert.IsNotNull(_container.GetRecord("health"));
            }

            [Test]
            public void GetRecordNull () {
                _container.Setup();

                StatDefinition nullDef = null;
                Assert.IsNull(_container.GetRecord(nullDef));
            }

            [Test]
            public void GetRecord () {
                _container.Setup();

                Assert.IsNotNull(_container.GetRecord(_health));
            }

            [Test]
            public void HasRecordByIdNullOrEmptyString () {
                _container.Setup();

                Assert.IsFalse(_container.HasRecord(""));

                var nullString = "";
                Assert.IsFalse(_container.HasRecord(nullString));
            }

            [Test]
            public void HasRecordById () {
                _container.Setup();

                Assert.IsTrue(_container.HasRecord("health"));
            }

            [Test]
            public void HasRecordNull () {
                _container.Setup();

                StatDefinition nullDef = null;
                Assert.IsFalse(_container.HasRecord(nullDef));
            }

            [Test]
            public void HasRecord () {
                _container.Setup();

                Assert.IsTrue(_container.HasRecord(_health));
            }

            [Test]
            public void GetStatNull () {
                _container.Setup();

                var nullString = "";
                Assert.AreEqual(0, _container.GetStat(nullString));
            }

            [Test]
            public void GetStat () {
                _container.Setup();

                Assert.AreEqual(3, _container.GetStat(_attack));
            }

            [Test]
            public void GetStatCurve () {
                _container.Setup();

                Assert.AreEqual(3, _container.GetStat(_armor, 3));
            }

            [Test]
            public void GetStatIntNull () {
                _container.Setup();

                var nullString = "";
                Assert.AreEqual(0, _container.GetStatInt(nullString));
            }

            [Test]
            public void GetStatInt () {
                _container.Setup();

                Assert.AreEqual(10, _container.GetStatInt(_health));
            }

            [Test]
            public void GetStatIntCurve () {
                _container.Setup();

                Assert.AreEqual(3, _container.GetStatInt(_armor, 3));
            }

            [Test]
            public void SetModifierDefinitionNullOrEmpty () {
                _container.Setup();

                StatDefinition def = null;
                _container.SetModifier(OperatorType.Add, def, "asdf", 2);
                _container.SetModifier(OperatorType.Add, "", "asdf", 2);
            }

            [Test]
            public void SetModifierIdNullOrEmpty () {
                _container.Setup();

                _container.SetModifier(OperatorType.Add, _health, null, 2);
                _container.SetModifier(OperatorType.Add, _health, "", 2);

                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _health, ""));
            }

            [Test]
            public void SetModifier () {
                _container.Setup();

                _container.SetModifier(OperatorType.Add, _health, "asdf", 2);

                Assert.AreEqual(2, _container.GetModifier(OperatorType.Add, _health, "asdf"));
            }

            [Test]
            public void GetModiferNullOrEmpty () {
                _container.Setup();

                StatDefinition def = null;
                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, def, "asdf"));
                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, "", "asdf"));
            }

            [Test]
            public void GetModifierIdNullOrEmpty () {
                _container.Setup();

                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _health, null));
                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _health, ""));
            }

            [Test]
            public void GetModifier () {
                _container.Setup();

                _container.SetModifier(OperatorType.Add, _health, "asdf", 2);

                Assert.AreEqual(2, _container.GetModifier(OperatorType.Add, _health, "asdf"));
            }

            [Test]
            public void GetModifierWhenModifierDoesNotExist () {
                _container.Setup();

                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _health, "asdf"));
            }

            [Test]
            public void RemoveModiferNullOrEmpty () {
                _container.Setup();

                StatDefinition def = null;
                Assert.IsFalse(_container.RemoveModifier(OperatorType.Add, def, "asdf"));
                Assert.IsFalse(_container.RemoveModifier(OperatorType.Add, "", "asdf"));
            }

            [Test]
            public void RemoveModifierIdNullOrEmpty () {
                _container.Setup();

                Assert.IsFalse(_container.RemoveModifier(OperatorType.Add, _health, null));
                Assert.IsFalse(_container.RemoveModifier(OperatorType.Add, _health, ""));
            }

            [Test]
            public void RemoveModifier () {
                _container.Setup();

                _container.SetModifier(OperatorType.Add, _health, "asdf", 2);

                Assert.IsTrue(_container.RemoveModifier(OperatorType.Add, _health, "asdf"));
                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _health, "asdf"));
            }

            [Test]
            public void RemoveModifierWhenModifierDoesNotExist () {
                _container.Setup();

                Assert.IsFalse(_container.RemoveModifier(OperatorType.Add, _health, "asdf"));
            }

            [Test]
            public void ClearAllModifiersNullList () {
                List<string> nullList = null;
                _container.ClearAllModifiers(nullList);
            }

            [Test]
            public void ClearAllModifiersByList () {
                _container.SetModifier(OperatorType.Add, _health, "asdf", 2);
                _container.SetModifier(OperatorType.Add, _armor, "asdf", 2);

                var list = new List<string> {"health", "armor"};
                _container.ClearAllModifiers(list);

                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _health, "asdf"));
                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _armor, "asdf"));
            }

            [Test]
            public void ClearAllModifiersById () {
                _container.SetModifier(OperatorType.Add, _health, "asdf", 2);
                _container.SetModifier(OperatorType.Add, _armor, "asdf", 2);

                _container.ClearAllModifiers("asdf");

                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _health, "asdf"));
                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _armor, "asdf"));
            }

            [Test]
            public void ClearAllModifiersByRecordAndModifier () {
                _container.SetModifier(OperatorType.Add, _health, "asdf", 2);

                _container.ClearAllModifiers(_container.GetRecord(_health), "asdf");

                Assert.AreEqual(0, _container.GetModifier(OperatorType.Add, _health, "asdf"));
            }
        }
    }
}
