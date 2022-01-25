using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPhysics2D;

[System.Serializable]
public struct CustomCollision2D
{
    public CustomBoxCollider collider;
    public Vector2 normal;
    public Vector2 contactPoint;

    public CustomCollision2D(CustomBoxCollider collider, Vector2 normal, Vector2 contactPoint)
    {
        this.collider = collider;
        this.normal = normal;
        this.contactPoint = contactPoint;
    }
}

public class PlayerController : PhysicalEntity
{
    [Header(nameof(PlayerController))]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float speed = 0.01f;
    [SerializeField] float friction = 0.01f;
    SpriteRenderer sprRenderer;
    public bool hasJumped;
    float xInput;
    int xDirection = 1;

    [Header("Shoot bubbles")]
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] float shootForce = 0.1f;
    PlayerAnimationManager animManager;
    float shootTimer;

    private void OnDrawGizmos()
    {
        if (boxCollider.box != null)
            boxCollider.Show();
    }

    public override void Awake()
    {
        base.Awake();
        sprRenderer = GetComponentInChildren<SpriteRenderer>();
        animManager = sprRenderer.GetComponent<PlayerAnimationManager>();
    }

    void Jump()
    {
        body.inAir = true;
        hasJumped = true;
        print("jump");
        EventManager.Instance.onPlayerJump.Invoke();
        body.velocity = new Vector2(body.velocity.x, 0);
        body.AddForce(Vector2.up * jumpForce);
    }

    public override void EnterBoxCollision(CustomCollision2D col)
    {
        base.EnterBoxCollision(col);

        if (col.normal.y > 0)
        {
            EventManager.Instance.onPlayerLanding.Invoke();
            hasJumped = false;
        }

        Bubble bubble = col.collider.GetComponent<Bubble>();
        if (bubble)
            Jump();
    }

    void ManageGraphics()
    {
        if (xInput < 0 && xDirection != -1)
            xDirection = -1;
        else if (xInput > 0 && xDirection != 1)
            xDirection = 1;

        Vector3 scale = transform.localScale;
        scale.x = xDirection;
        transform.localScale = scale;
    }

    void Movements()
    {
        if (xInput == 0)
            body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, 0, friction * Time.deltaTime), body.velocity.y);
        else
        {
            float velocity = speed * xInput;
            body.velocity = new Vector2(velocity, body.velocity.y);
        }
    }

    void Shoot()
    {
        animManager.Shoot();
        GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
        PhysicBody body = bubble.GetComponent<PhysicBody>();
        body.velocity = Vector2.right * xDirection * shootForce;
    }

    void ManageShooting()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > 0.25f)
        {
            if (Input.GetButtonDown("Shoot"))
            {
                Shoot();
                shootTimer = 0;
            }
        }
    }

    void InAir()
    {
        if (!body.inAir)
        {
            body.velocity = new Vector2(body.velocity.x, 0);
            if (Input.GetButtonDown("Jump"))
                Jump();
        }
    }

    public override void Update()
    {
        base.Update();
        xInput = Input.GetAxisRaw("Horizontal");
        ManageGraphics();
        ManageShooting();
        InAir();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Movements();
    }
}