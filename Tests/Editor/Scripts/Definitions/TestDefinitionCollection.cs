using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors.Testing {
	public class TestDefinitionCollection : TestBase {
		private StatDefinitionCollection _col1;
		private StatDefinitionCollection _col2;
		private StatDefinitionCollection _col3;

		private StatDefinition _stat1;
		private StatDefinition _stat2;
		private StatDefinition _stat3;

		[SetUp]
		public void SetupDefinitionCollection () {
			_col1 = ScriptableObject.CreateInstance<StatDefinitionCollection>();
			_col2 = ScriptableObject.CreateInstance<StatDefinitionCollection>();
			_col3 = ScriptableObject.CreateInstance<StatDefinitionCollection>();

			_stat1 = ScriptableObject.CreateInstance<StatDefinition>();
			_stat2 = ScriptableObject.CreateInstance<StatDefinition>();
			_stat3 = ScriptableObject.CreateInstance<StatDefinition>();
		}

		[TearDown]
		public void TeardownDefinitionCollection () {
			_col1 = null;
			_col2 = null;
			_col3 = null;

			_stat1 = null;
			_stat2 = null;
			_stat3 = null;
		}

		[Test]
		public void DefinitionCollectionReturnsBlankArrayIfEmpty () {
			Assert.AreEqual(0, _col1.GetDefinitions().Count);
		}

		[Test]
		public void DefinitionCollectionSkipsNullDefinitionValues () {
			_col1.definitions.Add(null);

			Assert.AreEqual(0, _col1.GetDefinitions().Count);
		}

		[Test]
		public void DefinitionReturnsNormally () {
			Assert.AreEqual(1, _stat1.GetDefinitions().Count);
		}

		[Test]
		public void DefinitionDoesNotReturnIfVisited () {
			Assert.AreEqual(0, _stat1.GetDefinitions(new HashSet<StatDefinitionBase> { _stat1 }).Count);
		}

		[Test]
		public void DefinitionCollectionDoesNotReturnIfVisited () {
			Assert.AreEqual(0, _col1.GetDefinitions(new HashSet<StatDefinitionBase> { _col1 }).Count);
		}

		[Test]
		public void DefinitionCollectionReturnsStats () {
			_col1.definitions.Add(_stat1);
			_col1.definitions.Add(_stat2);
			_col1.definitions.Add(_stat3);

			var results = _col1.GetDefinitions();

			Assert.AreEqual(3, results.Count);
			Assert.Contains(_stat1, results);
			Assert.Contains(_stat2, results);
			Assert.Contains(_stat3, results);
		}

		[Test]
		public void DefinitionCollectionReturnsDuplicateStatsAsOne () {
			_col1.definitions.Add(_stat1);
			_col1.definitions.Add(_stat2);
			_col1.definitions.Add(_stat2);

			var results = _col1.GetDefinitions();

			Assert.AreEqual(2, results.Count);
			Assert.Contains(_stat1, results);
			Assert.Contains(_stat2, results);
		}

		[Test]
		public void DefinitionCollectionReturnsCollections () {
			_col1.definitions.Add(_col2);
			_col1.definitions.Add(_col3);

			_col2.definitions.Add(_stat1);
			_col3.definitions.Add(_stat2);

			var results = _col1.GetDefinitions();

			Assert.AreEqual(2, results.Count);
			Assert.Contains(_stat1, results);
			Assert.Contains(_stat2, results);
		}

		[Test]
		public void DefinitionCollectionReturnsNestedCollections () {
			_col1.definitions.Add(_col2);
			_col2.definitions.Add(_col3);

			_col2.definitions.Add(_stat1);
			_col3.definitions.Add(_stat2);

			var results = _col1.GetDefinitions();

			Assert.AreEqual(2, results.Count);
			Assert.Contains(_stat1, results);
			Assert.Contains(_stat2, results);
		}

		[Test]
		public void DefinitionCollectionReturnsMultipleIdenticalCollections () {
			_col1.definitions.Add(_col2);
			_col1.definitions.Add(_col2);

			_col2.definitions.Add(_stat1);
			_col2.definitions.Add(_stat2);

			var results = _col1.GetDefinitions();

			Assert.AreEqual(2, results.Count);
			Assert.Contains(_stat1, results);
			Assert.Contains(_stat2, results);
		}

		[Test]
		public void DefinitionCollectionReturnsCollectionsWithIdenticalSiblingStats () {
			_col1.definitions.Add(_col2);
			_col1.definitions.Add(_stat1);

			_col2.definitions.Add(_stat1);
			_col2.definitions.Add(_stat2);

			var results = _col1.GetDefinitions();

			Assert.AreEqual(2, results.Count);
			Assert.Contains(_stat1, results);
			Assert.Contains(_stat2, results);
		}

		[Test]
		public void NestedReciprocalDefinitionCollectionsDoNotCrash () {
			_col1.definitions.Add(_col2);

			_col2.definitions.Add(_stat2);
			_col2.definitions.Add(_col1);
			_col2.definitions.Add(_stat3);

			var results = _col1.GetDefinitions();

			Assert.AreEqual(2, results.Count);
			Assert.Contains(_stat2, results);
			Assert.Contains(_stat3, results);
		}
	}
}
