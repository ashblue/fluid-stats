using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Adnc.StatsSystem.Editors.Testing {
	public abstract class TestSettingsBase : TestBase {
		protected StatsSettings settings;

		[SetUp]
		public void SetupSettingsBase () {
			settings = ScriptableObject.CreateInstance<StatsSettings>();
			StatsSettings.Current = settings;
		}

		[TearDown]
		public void TeardownSettingsBase () {
			StatsSettings.Current = null;
		}
	}
}

