using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    public float moveSpeed;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public bool isSprinting;
    [HideInInspector]

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] public float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    //References
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputManagement();
        
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            moveSpeed += 5;
            isSprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed -= 5;
            isSprinting = false;
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

    private IEnumerator Dash()
    {
        Collider2D collider = GetComponent<Collider2D>();
        canDash = false;
        isDashing = true;

        collider.isTrigger = true;
        rb.velocity = new Vector2(transform.localScale.x *moveDir.x * dashingPower, transform.localScale.y * moveDir.y * dashingPower);
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);
        collider.isTrigger = false;

        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && isDashing)
        {
            Destroy(collision.gameObject);
        }
    }
}

