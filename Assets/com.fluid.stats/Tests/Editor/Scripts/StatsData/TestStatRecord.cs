using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors.Testing {
    public class TestStatRecord : TestSettingsBase {
        [Test]
        public void CreateRecordWithoutDefinitionSilentlyErrors () {
            new StatRecord(null);
        }

        [Test]
        public void CreateRecordWithDefinition () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2);
            new StatRecord(d);
        }

        [Test]
        public void GetBaseValue () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2);
            var r = new StatRecord(d);

            Assert.AreEqual(2, r.GetValue());
        }

        [Test]
        public void GetBaseValueOverride () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2);

            var statOverride = new StatValueSelector { type = StatValueType.Int };
            var sInt = (StatValueInt)statOverride.GetValue();
            sInt.value = 8;

            var r = new StatRecord(d, statOverride);

            Assert.AreEqual(8, r.GetBaseValue());
        }

        [Test]
        public void GetValueWithoutModifier () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2);
            var r = new StatRecord(d);
            var result = r.GetValue();

            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetValueWithEveryModifer () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2);
            var r = new StatRecord(d);
            r.modifierAdd.Set("a", 4);
            r.modifierSubtract.Set("a", 2);
            r.modifierMultiply.Set("a", 3);
            r.modifierDivide.Set("a", 2);
            var result = r.GetValue();

            Assert.AreEqual(8, result);
        }

        [Test]
        public void GetValueRoundsForceRoundedDefinitions () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2.4f);
            SetPrivateField(d, "_roundResult", true);
            var r = new StatRecord(d);

            Assert.AreEqual(2, r.GetValue());
        }

        [Test]
        public void ModifierIsForceRoundedWithDefinitionForceRound () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2.4f);
            SetPrivateField(d, "_roundResult", true);
            SetPrivateField(d, "_roundModifiers", true);
            var r = new StatRecord(d);
            r.modifierAdd.Set("a", 2.4f);

            Assert.AreEqual(4, r.GetValue());
        }

        [Test]
        public void ModifierIsNotForceRoundedWhenDefinitionForceRoundFalse () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2.4f);
            SetPrivateField(d, "_roundModifiers", false);
            var r = new StatRecord(d);
            r.modifierAdd.Set("a", 2.4f);

            Assert.AreEqual(4.8f, r.GetValue());
        }

        [Test]
        public void ModifierNotForceRoundedWithoutOperatorForceRound () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2.4f);
            SetPrivateField(d, "_roundResult", true);
            var r = new StatRecord(d);
            r.modifierMultiply.Set("a", 0.05f);

            Assert.AreEqual(3, r.GetValue(3f));
        }

        [Test]
        public void GetValueUsesCustomOperatorOrderOnSettings () {
            var ooo = ScriptableObject.CreateInstance<OrderOfOperations>();
            var o = ooo.Operators[0];
            ooo.Operators.RemoveAt(0);
            ooo.Operators.Add(o);
            settings.OrderOfOperations = ooo;

            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 4f);
            var r = new StatRecord(d);
            r.modifierAdd.Set("a", 4);
            r.modifierSubtract.Set("a", 2);
            r.modifierMultiply.Set("a", 4);
            r.modifierDivide.Set("a", 2);

            var result = 4 - 2;
            result += result * 4;
            result /= 2;
            result += 4;

            Assert.AreEqual(result, r.GetValue());
        }

        [Test]
        public void GetValueTwiceReturnsCachedValue () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2);
            var r = new StatRecord(d);
            r.GetValue();

            Assert.AreEqual(2, r.GetValue());
        }

        [Test]
        public void ChangingModifierChangesCachedValueLookups () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2);
            var r = new StatRecord(d);
            r.modifierAdd.Set("a", 2f);
            r.GetValue();
            r.modifierAdd.Set("a", 4f);

            Assert.AreEqual(6, r.GetValue());
        }

        [Test]
        public void GetValueStoresLastRetrievedValue () {
            var d = TestStatValueSelector.CreateDefinition(StatValueType.Float, 2);
            var r = new StatRecord(d);
            r.modifierAdd.Set("a", 2f);
            r.GetValue();

            Assert.AreEqual(4, r.LastRetrievedValue);
        }
    }
}
