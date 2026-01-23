using UnityEngine;

public class HealthItemEffect : MonoBehaviour, IItemEffect
{
    [Header("Health Restore")]
    [SerializeField] private float healAmount = 25f;

    public void Apply(GameObject collector)
    {
        var healthSystem = collector.GetComponent<HealthSystem>();
        if (healthSystem == null) return;

        if (healthSystem.isDead) return;

        // Calcula cuánto puede curar realmente
        float missingHealth = healthSystem.MaxHealth - healthSystem.Health;
        if (missingHealth <= 0f) return;

        float finalHeal = Mathf.Min(healAmount, missingHealth);

        // Daño negativo = curación
        healthSystem.ReceivedDamage(-finalHeal);
    }
}
