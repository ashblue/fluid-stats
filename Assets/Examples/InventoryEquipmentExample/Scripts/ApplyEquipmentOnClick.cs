using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adnc.StatsSystem.Examples {
	public class ApplyEquipmentOnClick : MonoBehaviour {
		[SerializeField] EquipmentItemExample _item;

		public void ApplyEquipment () {
			_item.Equip();
		}
	}
}
