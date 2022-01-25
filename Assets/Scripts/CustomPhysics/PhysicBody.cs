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
        [SerializeField] Vector2 _velocity;
        [SerializeField] float drag = 2f;
        [SerializeField] PhysicalEntity entity;
        CustomBoxCollider customCollider;
        public bool inAir;
        public bool onWall;
        [SerializeField] List<CustomCollision2D> contacts = new List<CustomCollision2D>();

        Vector2 lastPos;
        Vector2 futurePos;
        Vector2 impulseVelocity;
        readonly Dictionary<PhysicBox, CustomCollision2D> contactCollisions = new Dictionary<PhysicBox, CustomCollision2D>();

        Vector2 ComputeVelocity => velocity + impulseVelocity;
        public Vector2 velocity
        {
            get => _velocity;
            set
            {
                if (value.y != _velocity.y)
                {
                    impulseVelocity.y = 0;
                }

                value.x *= Time.fixedDeltaTime;
                _velocity = value;
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                futurePos = (Vector2)transform.position + ComputeVelocity;

            Gizmos.DrawWireSphere(futurePos, 0.25f);
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
                if (inAir)
                    _velocity.y = gravityScale * Time.fixedDeltaTime * CustomPhysics.GravityValue;
                else
                    Drag();
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
                if (collision.normal.y > 0)
                    return true;
            }

            return false;
        }

        public void EnterBoxCollision(CustomCollision2D col)
        {
            if (entity)
                entity.EnterBoxCollision(col);

            contactCollisions.Add(col.collider.box, col);
            col.collider.isColliding = true;
            if (col.normal.y > 0 && !trigger)
            {
                _velocity.y = 0;
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
                foreach (var collider in PhysicsManager.instance.colliders)
                {
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
                if (item.normal.sqrMagnitude == 0)
                    continue;

                onWall = item.normal.x != 0;
                if (onWall)
                    _velocity.x = 0;
            }
        }

        void IntersectionOffset()
        {
            foreach (var item in contactCollisions.Values)
            {
                if (item.normal.y == 0)
                {
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
        }

        void ResolveCollision()
        {
            if (trigger)
                return;

            CheckWalls();
            IntersectionOffset();
        }

        public virtual void Update()
        {
            contacts = contactCollisions.Values.ToList();
            impulseVelocity.y = Mathf.Lerp(impulseVelocity.y, 0, drag * Time.fixedDeltaTime);

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
            futurePos = (Vector2)transform.position + ComputeVelocity;
            customCollider.box.position = futurePos;
            ManageCollisions();
            inAir = !CheckGround();
            transform.Translate(ComputeVelocity);
        }
    }
}