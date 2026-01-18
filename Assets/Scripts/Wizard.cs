using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackDamage;
    private HealthSystem healthSystem;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        anim = GetComponent<Animator>();
        StartCoroutine(AttackRoutine());

        if (healthSystem != null)
            healthSystem.OnDeath += OnDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            anim.SetTrigger("fireball");
            yield return new WaitForSeconds(attackTime);
        }
    }

    private void ThrowBall()
    {
        Instantiate(ball, spawnPoint.position, transform.rotation);
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}
