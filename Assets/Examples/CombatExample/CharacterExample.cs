using CleverCrow.Fluid.StatsSystem.Editors;
using CleverCrow.Fluid.StatsSystem.StatsContainers;
using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.Fluid.StatsSystem {
	[RequireComponent(typeof(StatsContainerExample))]
	public class CharacterExample : MonoBehaviour {
        private StatsContainer stats;

        [SerializeField]
        private Slider healthBar;

        void Start () {
			stats = GetComponent<StatsContainerExample>().copy;

			var health = stats.GetStat("health");
			healthBar.maxValue = health;
			healthBar.value = health;
		}

		void Update () {
			healthBar.maxValue = stats.GetStat("health");
		}

		public void ReceiveDamage (int damage) {
			healthBar.value -= Mathf.Max(0, damage - stats.GetStat("armor"));

			if (healthBar.value < 1) {
				Destroy(gameObject);
			}
		}

		public void AttackTarget (CharacterExample target) {
			if (target == null) return;

			target.ReceiveDamage((int)stats.GetStat("attack"));
		}

		public void AddHealth (float amount) {
			var health = stats.GetModifier(OperatorType.Add, "health", "example");
			stats.SetModifier(OperatorType.Add, "health", "example", health + amount);
		}

		public void RemoveHealth (float amount) {
			var health = stats.GetModifier(OperatorType.Subtract, "health", "example");
			stats.SetModifier(OperatorType.Subtract, "health", "example", health + amount);
		}

		public void MultiplyHealth (float amount) {
			var health = stats.GetModifier(OperatorType.Multiply, "health", "example");
			stats.SetModifier(OperatorType.Multiply, "health", "example", health + amount);
		}

		public void DivideHealth (float amount) {
			var health = stats.GetModifier(OperatorType.Divide, "health", "example");
			stats.SetModifier(OperatorType.Divide, "health", "example", health + amount);
		}

		public void RefillHealth () {
			healthBar.value = healthBar.maxValue;
		}

		public void ClearAll () {
			stats.ClearAllModifiers(stats.GetRecord("health"), "example");
		}
	}
}
