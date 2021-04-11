using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Adnc.StatsSystem.Editors {
	[CustomEditor(typeof(OrderOfOperations))]
	public class OrderOfOperationsEditor : Editor {
		private SortableListOperators _operators;

		private void OnEnable () {
			_operators = new SortableListOperators(this, "_operators", "Order of operations");
		}

		public override void OnInspectorGUI () {
			_operators.Update();
		}
	}
}

