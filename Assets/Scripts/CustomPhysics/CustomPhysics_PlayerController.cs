using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    [System.Serializable]
    public struct Collision
    {
        public CustomBoxCollider collider;
        public Vector2 normal;

        public Collision(CustomBoxCollider collider, Vector2 normal)
        {
            this.collider = collider;
            this.normal = normal;
        }
    }

    public class CustomPhysics_PlayerController : PhysicalEntity
    {
        [Header(nameof(CustomPhysics_PlayerController))]
        [SerializeField] float jumpForce = 5f;
        [SerializeField] float speed = 0.01f;
        [SerializeField] float friction = 0.01f;
        SpriteRenderer sprRenderer;
        float xInput;
        int xDirection = 1;

        private void OnDrawGizmos()
        {
            if (boxCollider.box != null)
                boxCollider.Show();
        }

        public override void Awake()
        {
            base.Awake();
            sprRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        void Jump()
        {
            inAir = true;
            print("jump");
            EventManager.Instance.onPlayerJump.Invoke();
            body.velocity.y = 0;
            body.velocity += Vector2.up * jumpForce;
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
                body.velocity.x = Mathf.Lerp(body.velocity.x, 0, friction * Time.deltaTime);
            else
            {
                float velocity = speed * xInput;
                velocity *= Time.fixedDeltaTime;
                body.velocity.x = velocity;
            }

            foreach (var item in contactCollisions.Values)
            {
                if (item.normal.sqrMagnitude == 0)
                    continue;

                bool onWallRight = item.normal.x < 0 && xDirection > 0;
                bool onWallLeft = item.normal.x > 0 && xDirection < 0;
                onWall = onWallLeft || onWallRight;
                if (onWall)
                    body.velocity.x = 0;
            }
        }

        public override void Landing()
        {
            base.Landing();
            EventManager.Instance.onPlayerLanding.Invoke();
        }

        public override void Update()
        {
            xInput = Input.GetAxisRaw("Horizontal");
            ManageGraphics();
            Movements();
            base.Update();

            if (!inAir)
            {
                body.velocity.y = 0;
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
            }
        }
    }
}