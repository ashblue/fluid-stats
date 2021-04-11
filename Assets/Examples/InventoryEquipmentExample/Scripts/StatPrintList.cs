using UnityEngine;
using System.Collections;
using Adnc.StatsSystem.Editors;

namespace Adnc.StatsSystem.Examples {
	public class StatPrintList : MonoBehaviour {
		[SerializeField] StatsContainerExample statTarget;
		[SerializeField] StatPrintListItem prefabLine;

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

