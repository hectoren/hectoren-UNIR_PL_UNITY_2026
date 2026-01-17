using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health;
    
    public void ReceivedDamage(float damageReceived)
    {
        health -= damageReceived;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
