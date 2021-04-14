using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Examples {
	[CreateAssetMenu(fileName = "EquipmentItemExample", menuName = "Fluid/Stats/Example/Equipment Item")]
	public class EquipmentItemExample : ScriptableObject {
		public string displayName;
		public PlayerEquipmentExample.Category equipmentType;
		public StatsAdjustment stats;

		public void Equip () {
			if (equipmentType == PlayerEquipmentExample.Category.Head) {
				if (PlayerEquipmentExample.Current.head == this) {
					PlayerEquipmentExample.Current.Unequip(this);
				} else {
					PlayerEquipmentExample.Current.Equip(this);
				}
			}

			if (equipmentType == PlayerEquipmentExample.Category.Armor) {
				if (PlayerEquipmentExample.Current.armor == this) {
					PlayerEquipmentExample.Current.Unequip(this);
				} else {
					PlayerEquipmentExample.Current.Equip(this);
				}
			}
		}
	}
}
