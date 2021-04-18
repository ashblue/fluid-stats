using CleverCrow.Fluid.StatsSystem;
using CleverCrow.Fluid.StatsSystem.StatsContainers;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthStat : MonoBehaviour {
    public Button buttonDealDamage;
    public Slider healthBar;

    public StatsContainer originalStats;
    public StatsContainer runtimeStats;

    public StatsAdjustment armor;

    private void Start () {
        // Generate a runtime copy that's safe to interact with
        runtimeStats = originalStats.CreateRuntimeCopy();

        armor.ApplyAdjustment(runtimeStats);

        // Generate the health bar
        var health = runtimeStats.GetStatInt("health");
        healthBar.maxValue = health;
        healthBar.value = health;

        // Bind our receive damage button
        buttonDealDamage.onClick.AddListener(ReceiveDamage);
    }

    private void ReceiveDamage () {
        healthBar.value -= 1;
    }
}
