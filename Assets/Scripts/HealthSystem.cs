using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health;
    public bool isDead = false;

    public event Action OnDamaged;
    public event Action OnDeath;
    
    public void ReceivedDamage(float damageReceived)
    {
        if (isDead) return;
        
        health -= damageReceived;
        if (health > 0f)
        {
            OnDamaged?.Invoke();
        }
        else
        {
            Kill();
        }
    }

    public void Kill()
    {
        if (isDead) return;

        health = 0f;
        isDead = true;

        OnDeath?.Invoke();
    }
}
