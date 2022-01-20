using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics_PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float speed = 0.01f;
    [SerializeField] float drag = 0.1f;
    [SerializeField] float friction = 0.1f;
    [SerializeField] Vector2 velocity;
    [SerializeField] Vector2 clampPosition;

    SpriteRenderer sprRenderer;
    bool flip;
    float xInput;

    private void Awake()
    {
        sprRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Jump()
    {
        print("jump");
        velocity.y = 0;
        velocity += Vector2.up * jumpForce;
    }

    void ManageGraphics()
    {
        if (xInput < 0 && flip)
            flip = false;
        else if (xInput > 0 && !flip)
            flip = true;

        sprRenderer.flipX = flip;
    }

    void Movements()
    {
        velocity += Vector2.right * xInput * speed * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x * Time.deltaTime, 0, friction * Time.deltaTime);
        velocity.y = Mathf.Lerp(velocity.y * Time.deltaTime, 0, drag * Time.deltaTime);
        transform.position += (Vector3)velocity;

        Vector2 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -clampPosition.x, clampPosition.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -clampPosition.y, clampPosition.y);
        transform.position = clampedPosition;
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        Movements();
        ManageGraphics();
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Jump();
    }
}
