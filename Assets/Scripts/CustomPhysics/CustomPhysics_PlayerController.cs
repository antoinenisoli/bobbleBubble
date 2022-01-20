using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class CustomPhysics_PlayerController : MonoBehaviour
    {
        [Header("Physics")]
        [SerializeField] float jumpForce = 5f;
        public bool inAir;
        [SerializeField] float speed = 0.01f;
        [SerializeField] float drag = 0.1f;
        [SerializeField] float friction = 0.1f;
        [SerializeField] Vector2 velocity;
        [SerializeField] Vector2 clampPosition;

        [SerializeField] CustomBoxCollider boxCollider;
        [SerializeField] PhysicBox raycastTest;
        CustomBoxCollider obstacleCollider;
        Vector2 normal;

        SpriteRenderer sprRenderer;
        bool flip;
        float xInput;

        private void OnDrawGizmos()
        {
            if (boxCollider.box != null)
            {
                boxCollider.Show();
                if (raycastTest != null)
                    boxCollider.Show(raycastTest);
            }
        }

        private void Awake()
        {
            sprRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        void Jump()
        {
            print("jump");
            EventManager.Instance.onPlayerJump.Invoke();
            velocity.y = 0;
            velocity += Vector2.up * jumpForce * Time.deltaTime;
            inAir = true;
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
            velocity.y = Mathf.Lerp(velocity.y, 0, drag * Time.deltaTime);

            transform.position += (Vector3)velocity;
            Vector2 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -clampPosition.x, clampPosition.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -clampPosition.y, clampPosition.y);
            transform.position = clampedPosition;
        }

        void Landing()
        {
            inAir = false;
            velocity.y = 0;
            EventManager.Instance.onPlayerLanding.Invoke();
        }

        void ManageCollisions()
        {
            if (boxCollider)
            {
                boxCollider.box.velocity = velocity;
                foreach (var collider in CustomPhysics2d_Manager.instance.colliders)
                {
                    if (collider == boxCollider)
                        return;

                    //print(CustomPhysics.CheckAABB(raycastTest, collider.box));
                    bool collision = CustomPhysics.CheckAABB(boxCollider.box, collider.box, out normal);
                    if (collision && collider.enabled)
                    {
                        if (normal.sqrMagnitude > 0)
                            print(normal);

                        if (!boxCollider.isColliding && inAir)
                            Landing();

                        boxCollider.isColliding = true;
                        collider.isColliding = true;
                        obstacleCollider = collider;
                        return;
                    }
                    else
                    {
                        obstacleCollider = null;
                        boxCollider.isColliding = false;
                        collider.isColliding = false;
                    }
                }
            }
        }

        void Update()
        {
            xInput = Input.GetAxisRaw("Horizontal");
            ManageCollisions();
            ManageGraphics();
            if (Input.GetKeyDown(KeyCode.UpArrow) && !inAir)
                Jump();

            Movements();
        }
    }
}