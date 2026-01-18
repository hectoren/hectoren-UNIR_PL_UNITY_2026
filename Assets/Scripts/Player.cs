using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private float inputH;

    [Header("Movement System")]
    [SerializeField] private Transform feet;
    [SerializeField] private float movementVelocity = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float distanceDetectionGround = 0.2f;
    [SerializeField] private LayerMask jumpedLayer;

    [Header("Combat System")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private LayerMask damageableLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    /* ================= INPUT EVENTS ================= */

    public void OnMove(InputAction.CallbackContext context)
    {
        // Leemos el Vector2 del input (teclado o control)
        Vector2 input = context.ReadValue<Vector2>();
        inputH = input.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (OnGround())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        anim.SetTrigger("attack_1");
    }

    /* ================= MOVEMENT ================= */

    void FixedUpdate()
    {
        rb.velocity = new Vector2(inputH * movementVelocity, rb.velocity.y);

        anim.SetBool("running", inputH != 0);

        if (inputH > 0)
            transform.eulerAngles = Vector3.zero;
        else if (inputH < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    private bool OnGround()
    {
        return Physics2D.Raycast(
            feet.position,
            Vector2.down,
            distanceDetectionGround,
            jumpedLayer
        );
    }

    // Este método suele llamarse desde un Animation Event
    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            damageableLayer
        );

        foreach (Collider2D hit in hits)
        {
            HealthSystem hs = hit.GetComponent<HealthSystem>();
            if (hs != null)
                hs.ReceivedDamage(attackDamage);
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
