using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class CustomPhysicBody : MonoBehaviour
    {
        [SerializeField] float gravityScale = 1f;
        public Vector2 velocity;
        CustomBoxCollider customCollider;
        [SerializeField] PhysicalEntity entity;

        private void Awake()
        {
            customCollider = GetComponent<CustomBoxCollider>();
        }

        private void Update()
        {
            if (customCollider)
            {
                if (entity)
                {
                    if (entity.inAir || (customCollider.isColliding && entity.onWall))
                        velocity = CustomPhysics2d_Manager.instance.baseGravity * gravityScale * Time.deltaTime;
                    else
                        velocity = Vector2.zero;
                }
                else
                {
                    if (!customCollider.isColliding)
                        velocity = CustomPhysics2d_Manager.instance.baseGravity * gravityScale * Time.deltaTime;
                    else
                        velocity = Vector2.zero;
                }
            }

            transform.position += (Vector3)velocity;
        }
    }
}