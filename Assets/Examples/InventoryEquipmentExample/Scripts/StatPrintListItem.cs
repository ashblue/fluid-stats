using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.Fluid.StatsSystem.Examples {
	public class StatPrintListItem : MonoBehaviour {
		[SerializeField] Text _name;
		public string Name {
			set {
				_name.text = value;
			}
		}

		[SerializeField] Text val;
		public string Value {
			set {
				val.text = value;
			}
		}

		[HideInInspector] public StatRecord record;

		void Update () {
			if (record != null) {
				// @NOTE Look at the record.DisplayType to customize how values are shown
				if (record.Definition.IsPercentile) {
					Value = string.Format("{0:0%}", record.GetValue());
				} else {
					Value = record.GetValue().ToString();
				}
			}
		}
	}
}

