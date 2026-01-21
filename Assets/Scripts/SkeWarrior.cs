using System.Collections;
using UnityEngine;

public class SkeWarrior : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float speedPatrol = 2f;

    [Header("Combat")]
    [SerializeField] private float attackDamage = 10f;

    [Header("Hurt / Death")]
    [SerializeField] private float hurtStunTime = 0.3f;
    [SerializeField] private float deathDestroyDelay = 1.2f;

    private Vector3 currentDestination;
    private int currentIndex = 0;

    private HealthSystem healthSystem;
    private Animator anim;

    private Coroutine patrolRoutine;
    private bool isStunned;
    private bool isDead;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        anim = GetComponent<Animator>();

        currentDestination = wayPoints[currentIndex].position;
        patrolRoutine = StartCoroutine(Patrol());

        if (healthSystem != null)
        {
            healthSystem.OnDamaged += OnHurt;
            healthSystem.OnDeath += OnDeath;
        }
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            while (transform.position != currentDestination)
            {
                if (isDead)
                    yield break;

                if (isStunned)
                {
                    yield return null;
                    continue;
                }

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    currentDestination,
                    speedPatrol * Time.deltaTime
                );

                yield return null;
            }

            DefineNewDestination();
        }
    }

    private void DefineNewDestination()
    {
        currentIndex++;
        if (currentIndex >= wayPoints.Length)
            currentIndex = 0;

        currentDestination = wayPoints[currentIndex].position;
        FocusDestination();
    }

    private void FocusDestination()
    {
        if (currentDestination.x > transform.position.x)
            transform.localScale = Vector3.one;
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("DetectionPlayer"))
        {
            Debug.Log("Player Detectado!!!");
        }
        else if (other.CompareTag("PlayerHitBox"))
        {
            Debug.Log("Player Atravesado!!!");
            HealthSystem hs = other.GetComponent<HealthSystem>();
            if (hs != null)
                hs.ReceivedDamage(attackDamage);
        }
    }

    // =====================
    // HURT
    // =====================
    private void OnHurt()
    {
        if (isDead || isStunned) return;

        anim.SetTrigger("hurt");
        StartCoroutine(HurtStun());
    }

    private IEnumerator HurtStun()
    {
        isStunned = true;
        yield return new WaitForSeconds(hurtStunTime);
        isStunned = false;
    }

    // =====================
    // DEATH
    // =====================
    private void OnDeath()
    {
        if (isDead) return;
        isDead = true;

        anim.SetBool("isDead", true);

        if (patrolRoutine != null)
            StopCoroutine(patrolRoutine);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, deathDestroyDelay);
    }
}
