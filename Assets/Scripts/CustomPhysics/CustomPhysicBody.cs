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

        private void Awake()
        {
            customCollider = GetComponent<CustomBoxCollider>();
        }

        private void Update()
        {
            if (!customCollider.isColliding)
                velocity = CustomPhysics2d_Manager.instance.baseGravity * gravityScale * Time.deltaTime;
            else
                velocity = Vector2.zero;

            transform.position += (Vector3)velocity;
        }
    }
}