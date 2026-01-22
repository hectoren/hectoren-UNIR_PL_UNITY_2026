using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;

    public bool isDead = false;

    // Exposición segura para UI
    public float Health => health;
    public float MaxHealth => maxHealth;

    public event Action OnDamaged;
    public event Action OnDeath;

    // Evento específico para UI
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        if (maxHealth <= 0f)
            maxHealth = health;

        health = Mathf.Clamp(health, 0f, maxHealth);
        isDead = false;
    }

    public void ReceivedDamage(float damageReceived)
    {
        if (isDead) return;

        health -= damageReceived;
        health = Mathf.Clamp(health, 0f, maxHealth);

        OnDamaged?.Invoke();

        OnHealthChanged?.Invoke(health, maxHealth);

        if (health <= 0f)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if (isDead) return;

        health = 0f;
        isDead = true;

        // Asegurar actualización final de UI
        OnHealthChanged?.Invoke(health, maxHealth);

        OnDeath?.Invoke();
    }
}
