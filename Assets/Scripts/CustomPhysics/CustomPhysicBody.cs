using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class CustomPhysicBody : MonoBehaviour
    {
        [SerializeField] float gravityScale = 1f;
        public Vector2 velocity;
        [SerializeField] Vector2 maxVelocity;
        [SerializeField] float drag = 0.1f;
        public bool gravity = true;
        CustomBoxCollider customCollider;
        [SerializeField] PhysicalEntity entity;

        private void Awake()
        {
            customCollider = GetComponent<CustomBoxCollider>();
        }

        void Drag()
        {
            velocity.y = Mathf.Lerp(velocity.y, 0, drag * Time.deltaTime);
        }

        void ApplyGravity()
        {
            if (!gravity)
                return;

            if (customCollider)
            {
                if (entity)
                {
                    if (entity.inAir)
                        velocity += gravityScale * Time.deltaTime * CustomPhysics2d_Manager.instance.baseGravity;
                    else
                        Drag();
                }
                else
                {
                    if (!customCollider.isColliding)
                        velocity += gravityScale * Time.deltaTime * CustomPhysics2d_Manager.instance.baseGravity;
                    else
                        Drag();
                }

                if (velocity.y < maxVelocity.y)
                    velocity.y = maxVelocity.y;
            }
        }

        private void Update()
        {
            ApplyGravity();
            transform.position += (Vector3)velocity;
        }
    }
}