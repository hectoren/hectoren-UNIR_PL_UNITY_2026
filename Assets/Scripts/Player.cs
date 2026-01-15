using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float inputH;
    private Animator anim;
    [SerializeField] private float movementVelocity;
    [SerializeField] private float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        Jump();

        Attack_1();
    }

    private void Attack_1()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack_1");
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }

    private void Movement()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputH * movementVelocity, rb.velocity.y);

        if (inputH != 0)
        {
            anim.SetBool("running", true);
            if (inputH > 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            anim.SetBool("running", false);
        }
    }
}
