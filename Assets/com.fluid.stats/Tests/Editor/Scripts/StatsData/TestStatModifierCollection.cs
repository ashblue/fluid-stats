using System.Linq;
using NUnit.Framework;

namespace CleverCrow.Fluid.StatsSystem.Editors.Testing {
    public class TestStatModifierCollection : TestSettingsBase {
        public class ModifyValueMethod : TestSettingsBase {
            public class Multiply : ModifyValueMethod {
                private StatModifierCollection _col;

                [SetUp]
                public void BeforeEachMethod () {
                    _col = new StatModifierCollection(OperatorType.Multiply);
                }

                [Test]
                public void Negative_removes_a_percentage () {
                    _col.Set("a", -0.05f);

                    Assert.AreEqual(95, _col.ModifyValue(100));
                }

                [Test]
                public void Two_negatives_are_combined_before_removing () {
                    _col.Set("a", -0.05f);
                    _col.Set("b", -0.05f);

                    Assert.AreEqual(90, _col.ModifyValue(100));
                }

                [Test]
                public void Positive_adds_a_percentage () {
                    _col.Set("a", 0.05f);

                    Assert.AreEqual(105, _col.ModifyValue(100));
                }

                [Test]
                public void Two_positive_are_combined_before_adding () {
                    _col.Set("a", 0.05f);
                    _col.Set("b", 0.05f);

                    Assert.AreEqual(110, _col.ModifyValue(100));
                }

                [Test]
                public void Negative_greater_than_1_does_not_go_below_zero () {
                    _col.Set("a", -2);

                    Assert.AreEqual(0, _col.ModifyValue(100));
                }

                [Test]
                public void Cancelling_pos_neg_does_nothing () {
                    _col.Set("a", -0.15f);
                    _col.Set("b", 0.15f);

                    Assert.AreEqual(100, _col.ModifyValue(100));
                }

                [Test]
                public void Negative_and_positive_values_overlap () {
                    _col.Set("a", -0.15f);
                    _col.Set("b", 0.25f);

                    Assert.AreEqual(110, _col.ModifyValue(100));
                }
            }
        }

        [Test]
        public void GetNull () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            var result = modCol.Get(null);

            Assert.IsNull(result);
        }

        [Test]
        public void GetById () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            modCol.Set("asdf", 22);
            var result = modCol.Get("asdf");

            Assert.AreEqual(22, result.value);
        }

        [Test]
        public void GetByMissingId () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            var result = modCol.Get("asdf");

            Assert.IsNull(result);
        }

        [Test]
        public void SetById () {
            GetById();
        }

        [Test]
        public void SetByIdNull () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            modCol.Set(null, 22);
        }

        [Test]
        public void SetByIdChangeValue () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            modCol.Set("asdf", 22);
            modCol.Set("asdf", 11.5f);
            var result = modCol.Get("asdf");

            Assert.AreEqual(11.5f, result.value);
        }

        [Test]
        public void SetByIdAutoRound () {
            var modCol = new StatModifierCollection(OperatorType.Add) { forceRound = true };
            modCol.Set("asdf", 22.4f);
            var result = modCol.Get("asdf");

            Assert.AreEqual(22, result.value);
        }

        [Test]
        public void SetByIdAddsToList () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            modCol.Set("asdf", 22.4f);

            Assert.IsTrue(modCol.ListValues.Any(v => v.id == "asdf"));
        }

        [Test]
        public void SetTriggersDirty () {
            var dirtyTriggered = false;
            var modCol = new StatModifierCollection(OperatorType.Add);
            modCol.onDirty += () => { dirtyTriggered = true; };
            modCol.Set("asdf", 22);

            Assert.IsTrue(dirtyTriggered);
        }

        [Test]
        public void RemoveById () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            modCol.Set("asdf", 22.4f);
            modCol.Remove("asdf");

            Assert.IsFalse(modCol.ListValues.Any(v => v.id == "asdf"));
        }

        [Test]
        public void RemoveByIdMissing () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            modCol.Remove("asdf");
        }

        [Test]
        public void RemoveByIdNull () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            modCol.Remove(null);
        }

        [Test]
        public void ModifyValueAdd () {
            var modCol = new StatModifierCollection(OperatorType.Add);
            modCol.Set("a", 2);
            modCol.Set("b", 3);

            var result = modCol.ModifyValue(1);

            Assert.AreEqual(6, result);
        }

        [Test]
        public void ModifyValueSubtract () {
            var modCol = new StatModifierCollection(OperatorType.Subtract);
            modCol.Set("a", 2);
            modCol.Set("b", 3);

            var result = modCol.ModifyValue(10);

            Assert.AreEqual(5, result);
        }

        [Test]
        public void ModifyValueMultiply () {
            var modCol = new StatModifierCollection(OperatorType.Multiply);
            modCol.Set("a", 2);
            modCol.Set("b", 3);

            var result = modCol.ModifyValue(2);

            Assert.AreEqual(12, result);
        }

        [Test]
        public void ModifyValueDivide () {
            var modCol = new StatModifierCollection(OperatorType.Divide);
            modCol.Set("a", 2);
            modCol.Set("b", 4);

            var result = modCol.ModifyValue(10);

            Assert.AreEqual(10f / 2f / 4f, result);
        }
    }
}
