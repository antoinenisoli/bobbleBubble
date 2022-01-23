using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class PhysicBody : MonoBehaviour
    {
        [SerializeField] float gravityScale = 1f;
        [SerializeField] Vector2 _velocity;
        Vector2 impulseVelocity;
        [SerializeField] float drag = 0.1f;
        public bool gravity = true;
        CustomBoxCollider customCollider;
        [SerializeField] PhysicalEntity entity;

        public Vector2 velocity
        {
            get => _velocity;
            set
            {
                if (value.y != _velocity.y)
                    impulseVelocity.y = 0;

                _velocity = value;
            }
        }
        Vector2 RealVelocity
        {
            get
            {
                return velocity + impulseVelocity;
            }
        }

        private void Awake()
        {
            customCollider = GetComponent<CustomBoxCollider>();
        }

        public void AddForce(Vector2 force)
        {
            impulseVelocity = force;
        }

        void Drag()
        {
            _velocity.y = 0;
            impulseVelocity.y = 0;
        }

        void ApplyGravity()
        {
            if (!gravity)
                return;

            if (customCollider)
            {
                if (entity)
                {
                    if (entity.inAir || (entity.inAir && customCollider.isColliding))
                        _velocity.y = gravityScale * Time.fixedDeltaTime * -9.81f;
                    else
                        Drag();
                }
                else
                {
                    if (!customCollider.isColliding)
                        _velocity.y = gravityScale * Time.fixedDeltaTime * -9.81f;
                    else
                        Drag();
                }
            }
        }

        private void Update()
        {
            impulseVelocity.y = Mathf.Lerp(impulseVelocity.y, 0, drag * Time.fixedDeltaTime);
            transform.Translate(RealVelocity);
        }

        private void FixedUpdate()
        {
            ApplyGravity();
        }
    }
}