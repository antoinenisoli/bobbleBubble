using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomPhysics2D
{
    public class PhysicBody : MonoBehaviour
    {
        public bool trigger;
        [SerializeField] bool gravity = true;
        [SerializeField] float gravityScale = 0.15f;
        [SerializeField] float acceleration = 0.05f;
        [SerializeField] float drag = 0.03f;
        [SerializeField] PhysicalEntity entity;
        CustomBoxCollider customCollider;
        public bool inAir;
        public bool onWall;
        [SerializeField] List<CustomCollision2D> contacts = new List<CustomCollision2D>();

        Vector2 lastPos;
        Vector2 futurePos;
        [SerializeField] Vector2 impulseVelocity;
        readonly Dictionary<PhysicBox, CustomCollision2D> contactCollisions = new Dictionary<PhysicBox, CustomCollision2D>();

        public Vector2 velocity;
        public bool hasJumped;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                futurePos = (Vector2)transform.position + velocity;

            Gizmos.DrawWireSphere(futurePos, 0.25f);
        }

        private void Awake()
        {
            customCollider = GetComponent<CustomBoxCollider>();
        }

        public void AddForce(Vector2 force)
        {
            if (force.y > 0)
                hasJumped = true;

            impulseVelocity = force;
        }

        void ApplyGravity()
        {
            if (!gravity)
                return;

            if (customCollider)
            {
                if (hasJumped)
                {
                    if (velocity.y < impulseVelocity.y)
                        velocity.y += acceleration * Time.fixedDeltaTime;
                    else
                    {
                        velocity.y = impulseVelocity.y;
                        hasJumped = false;
                    }
                }
                else if (inAir)
                {
                    if (velocity.y > gravityScale * CustomPhysics.GravityValue * Time.fixedDeltaTime) 
                        velocity.y -= drag * Time.fixedDeltaTime;
                    else 
                        velocity.y = gravityScale * CustomPhysics.GravityValue * Time.fixedDeltaTime;
                }
            }
        }

        public bool IsColliding()
        {
            return contactCollisions.Count > 0;
        }

        public bool CheckGround()
        {
            foreach (var collision in contactCollisions.Values)
            {
                if (collision.collider == null)
                    continue;
                else
                {
                    if (!CanCollide(collision.collider.gameObject))
                        continue;
                    if (collision.normal.y > 0)
                        return true;
                }
            }

            return false;
        }

        public bool CanCollide(GameObject other)
        {
            return !trigger && CustomPhysics.CompareLayer(entity.interactWith, other.layer);
        }

        public void EnterBoxCollision(CustomCollision2D col)
        {
            if (entity)
                entity.EnterBoxCollision(col);

            contactCollisions.Add(col.collider.box, col);
            col.collider.isColliding = true;
            if (col.normal.y > 0 && CanCollide(col.collider.gameObject) && col.collider.gameObject.activeSelf)
            {
                velocity.y = 0;
                transform.position = new Vector2(transform.position.x, col.collider.box.maxY + customCollider.box.height / 2);
            }
        }

        public void ExitBoxCollision(CustomBoxCollider collider)
        {
            if (entity)
                entity.ExitBoxCollision(contactCollisions[collider.box]);

            contactCollisions.Remove(collider.box);
            collider.isColliding = false;
        }

        public void ManageCollisions()
        {
            if (customCollider)
            {
                for (int i = 0; i < GameplayManager.instance.colliders.Count; i++)
                {
                    CustomBoxCollider collider = GameplayManager.instance.colliders[i];
                    if (collider == null)
                        continue;
                    if (collider == customCollider)
                        continue;

                    bool collision = CustomPhysics.CheckAABB(customCollider.box, collider.box, out Vector2 normal);
                    if (collision)
                    {
                        if (!contactCollisions.ContainsKey(collider.box))
                        {
                            Vector2 point = CustomPhysics.GetCollisionPoint(customCollider.box, collider.box);
                            //Debug.DrawLine(transform.position, point, Color.red, 5f);
                            CustomCollision2D col = new CustomCollision2D(collider, normal, point);
                            EnterBoxCollision(col);
                        }
                    }
                    else if (contactCollisions.ContainsKey(collider.box))
                        ExitBoxCollision(collider);
                }
            }
        }

        void CheckWalls()
        {
            foreach (var item in contactCollisions.Values)
            {
                if (item.normal.sqrMagnitude == 0 || !CanCollide(item.collider.gameObject))
                    continue;

                onWall = item.normal.x != 0;
                if (onWall)
                    velocity.x = 0;
            }
        }

        void IntersectionOffset()
        {
            foreach (var item in contactCollisions.Values)
            {
                if (item.normal.y != 0 || !CanCollide(item.collider.gameObject))
                    continue;

                if (item.normal.x > 0)
                {
                    float diff = customCollider.box.minX - item.collider.box.maxX;
                    transform.position += Vector3.left * diff;
                }
                else if (item.normal.x < 0)
                {
                    float diff = customCollider.box.maxX - item.collider.box.minX;
                    transform.position -= Vector3.right * diff;
                }
            }
        }

        void ResolveCollision()
        {
            if (trigger)
                return;

            CheckWalls();
            IntersectionOffset();
        }

        void RemoveWrongColliders()
        {
            foreach (var item in contactCollisions.Values)
            {
                if (!item.collider || !item.collider.gameObject.activeSelf)
                {
                    contactCollisions.Remove(item.collider.box);
                    return;
                }
            }
        }

        public virtual void Update()
        {
            RemoveWrongColliders();
            contacts = contactCollisions.Values.ToList();

            if (lastPos != (Vector2)transform.position)
            {
                ResolveCollision();
                lastPos = transform.position;
            }
        }

        public virtual void FixedUpdate()
        {
            customCollider.isColliding = IsColliding();
            ApplyGravity();
            futurePos = (Vector2)transform.position + velocity;
            customCollider.box.position = futurePos;
            ManageCollisions();
            inAir = !CheckGround();

            transform.Translate(velocity);
        }
    }
}