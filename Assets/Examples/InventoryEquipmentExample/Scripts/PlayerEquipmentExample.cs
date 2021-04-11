using UnityEngine;
using System.Collections.Generic;
using Adnc.StatsSystem.Editors;
using AdncStats.Scripts.StatsContainer;

namespace Adnc.StatsSystem.Examples {
	[RequireComponent(typeof(StatsContainerExample))]
	public class PlayerEquipmentExample : MonoBehaviour {
		StatsContainer stats;
		public EquipmentItemExample head;
		public EquipmentItemExample armor;

		[Header("Setup")]
		[SerializeField] EquipmentItemExample equipHeadOnLoad;
		[SerializeField] EquipmentItemExample equipArmorOnLoad;

		public enum Category {
			Head,
			Armor
		}

		static PlayerEquipmentExample _current;
		public static PlayerEquipmentExample Current {
			get {
				return _current;
			}
		}

		void Awake () {
			_current = this;
			stats = GetComponent<StatsContainerExample>().copy;

			Debug.LogFormat("Health before equipping {0}", stats.GetStatInt("health"));

			if (equipHeadOnLoad != null) {
				Equip(equipHeadOnLoad);
			}

			if (equipArmorOnLoad != null) {
				Equip(equipArmorOnLoad);
			}

			Debug.LogFormat("Health after equipping {0}", stats.GetStatInt("health"));
		}

		public void Equip (EquipmentItemExample item) {
			if (item.equipmentType == Category.Head) {
				if (head != null) head.stats.RemoveAdjustment(stats);

				head = item;
				if (head != null) head.stats.ApplyAdjustment(stats);

			} else if (item.equipmentType == Category.Armor) {
				if (armor != null) armor.stats.RemoveAdjustment(stats);

				armor = item;
				if (armor != null) armor.stats.ApplyAdjustment(stats);
			}
		}

		public void Unequip (EquipmentItemExample item) {
			if (item.equipmentType == Category.Head && item == head) {
				if (head != null) head.stats.RemoveAdjustment(stats);
				head = null;
			} else if (item.equipmentType == Category.Armor && item == armor) {
				if (armor != null) armor.stats.RemoveAdjustment(stats);
				armor = null;
			}
		}
	}
}

