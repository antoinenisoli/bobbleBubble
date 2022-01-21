using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public struct Collision
    {
        public PhysicBox box;
        public Vector2 normal;

        public Collision(PhysicBox box, Vector2 normal)
        {
            this.box = box;
            this.normal = normal;
        }
    }

    public class CustomPhysics_PlayerController : PhysicalEntity
    {
        [Header(nameof(CustomPhysics_PlayerController))]
        [SerializeField] [Range(0, 0.1f)] float jumpForce = 5f;
        [SerializeField] float speed = 0.01f;
        [SerializeField] float drag = 0.1f;
        SpriteRenderer sprRenderer;
        bool flip;
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
            print("jump");
            EventManager.Instance.onPlayerJump.Invoke();
            localVelocity.y = 0;
            localVelocity += Vector2.up * jumpForce;
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

        void LerpVelocity()
        {
            localVelocity.x *= Time.deltaTime;
            localVelocity.y = Mathf.Lerp(localVelocity.y, 0, drag * Time.deltaTime);
            transform.position += (Vector3)localVelocity;
            body.velocity = localVelocity;
        }

        void Movements()
        {
            localVelocity += Vector2.right * xInput * speed;
            foreach (var item in contactCollisions.Values)
            {
                if (normal.sqrMagnitude == 0)
                    continue;

                bool onWallRight = item.normal.x < 0 && xDirection > 0;
                bool onWallLeft = item.normal.x > 0 && xDirection < 0;
                onWall = onWallLeft || onWallRight;
                if (onWall)
                    localVelocity.x = 0;
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
            if (Input.GetKeyDown(KeyCode.UpArrow) && !inAir)
                Jump();

            Movements();
            LerpVelocity();
            base.Update();

            print(CheckGround());
            inAir = !CheckGround();
        }
    }
}