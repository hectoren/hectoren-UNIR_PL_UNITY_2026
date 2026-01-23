using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingDemon : EnemyBase
{
    [Header("Patrol")]
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float waypointReachDistance = 0.1f;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 4f;

    [Header("Attack")]
    [SerializeField] private float attackDistance = 1.2f;
    [SerializeField] private float attackCooldown = 0.8f;

    private Rigidbody2D rb;
    private Transform player;

    private Vector2 currentDestination;
    private int currentIndex;

    private bool playerDetected;
    private bool isAttacking;
    private float nextAttackTime;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject playerHitBox = GameObject.FindGameObjectWithTag("PlayerHitBox");
        if (playerHitBox != null)
            player = playerHitBox.transform;

        if (wayPoints != null && wayPoints.Length > 0)
        {
            currentIndex = 0;
            currentDestination = wayPoints[currentIndex].position;
            FocusTarget(currentDestination);
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (playerDetected && player != null)
            FocusTarget(player.position);

        if (isAttacking)
        {
            if (Time.time >= nextAttackTime)
                isAttacking = false;

            rb.velocity = Vector2.zero;
            return;
        }


        if (playerDetected && player != null)
            HandleChaseAndAttack();
        else
            HandlePatrol();
    }

    private void HandlePatrol()
    {
        anim.SetBool("isFlying", true);

        if (Vector2.Distance(rb.position, currentDestination) <= waypointReachDistance)
            DefineNewDestination();

        MoveTo(currentDestination, patrolSpeed);
    }

    private void DefineNewDestination()
    {
        currentIndex++;
        if (currentIndex >= wayPoints.Length)
            currentIndex = 0;

        currentDestination = wayPoints[currentIndex].position;
        FocusTarget(currentDestination);
    }

    private void HandleChaseAndAttack()
    {
        float dist = Vector2.Distance(rb.position, player.position);

        if (dist <= attackDistance && Time.time >= nextAttackTime)
        {
            StartAttack();
            return;
        }

        anim.SetBool("isFlying", true);
        MoveTo(player.position, chaseSpeed);
    }

    private void StartAttack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;

        if (player != null)
            FocusTarget(player.position);

        anim.SetTrigger("attack");
        nextAttackTime = Time.time + attackCooldown;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    private void MoveTo(Vector2 target, float speed)
    {
        rb.MovePosition(
            Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime)
        );
    }

    private void FocusTarget(Vector2 target)
    {
        transform.localScale = (target.x >= transform.position.x)
            ? new Vector3(-1f, 1f, 1f)
            : Vector3.one;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitBox"))
        {
            playerDetected = true;

            if (player != null)
                FocusTarget(player.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitBox"))
        {
            playerDetected = false;
        }
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1.2f);
    }

}
