using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Examples {
	public class ApplyEquipmentOnClick : MonoBehaviour {
		[SerializeField]
        private EquipmentItemExample _item;

		public void ApplyEquipment () {
			_item.Equip();
		}
	}
}
