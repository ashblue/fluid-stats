using UnityEngine;

namespace Adnc.StatsSystem.Examples {
	public class ApplyEquipmentOnClick : MonoBehaviour {
		[SerializeField]
        private EquipmentItemExample _item;

		public void ApplyEquipment () {
			_item.Equip();
		}
	}
}
