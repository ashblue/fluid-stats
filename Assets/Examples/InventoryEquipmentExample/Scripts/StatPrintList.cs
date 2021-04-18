using UnityEngine;
using System.Collections;
using CleverCrow.Fluid.StatsSystem.Editors;

namespace CleverCrow.Fluid.StatsSystem.Examples {
	public class StatPrintList : MonoBehaviour {
		[SerializeField]
        private StatsContainerExample statTarget;

		[SerializeField]
        private StatPrintListItem prefabLine;

		void Start () {
			foreach (var r in statTarget.copy.records.records) {
				if (r.Definition.Hidden) continue;

				var item = Instantiate(prefabLine);
				item.transform.SetParent(transform);
				item.Name = r.Definition.DisplayName;
				item.record = r;
			}
		}
	}
}

