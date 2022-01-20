using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movements")]
    public float jumpForce = 5f;
    public float speed = 10f;
    Rigidbody2D rb;
    Vector3 velocity;

    [Header("Ground detection")]
    public Transform feet;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDistance = 1f;
    bool onGround;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(feet.position, Vector2.down * groundDistance);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Jump()
    {
        print("jump");
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        rb.velocity = velocity;
        rb.AddForce(Vector2.up * jumpForce);
    }

    private void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        velocity = rb.velocity;
        velocity.x = xInput * speed;

        if (Input.GetKeyDown(KeyCode.UpArrow) && onGround)
            Jump();
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
        onGround = Physics2D.Raycast(feet.position, Vector2.down, groundDistance, groundLayer);
    }
}
