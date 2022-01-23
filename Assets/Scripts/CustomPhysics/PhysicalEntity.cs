using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomPhysics2D
{
    public class PhysicalEntity : MonoBehaviour
    {
        [Header(nameof(PhysicalEntity))]
        [SerializeField] protected CustomBoxCollider boxCollider;
        public bool inAir;
        public bool onWall;
        [SerializeField] List<Collision> contacts = new List<Collision>();
        protected Dictionary<PhysicBox, Collision> contactCollisions = new Dictionary<PhysicBox, Collision>();
        protected PhysicBody body;

        public virtual void Awake()
        {
            body = GetComponent<PhysicBody>();
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

        public virtual void CollisionWithBox(Collision col)
        {
            body.velocity = new Vector2(body.velocity.x, 0);
            print(col.collider.GetInstanceID());
            contactCollisions.Add(col.collider.box, col);
            col.collider.isColliding = true;

            if (col.normal.y > 0)
                transform.position = new Vector2(transform.position.x, col.collider.box.maxY + boxCollider.box.height/2);
            else if (col.normal.y < 0)
                transform.position = new Vector2(transform.position.x, col.collider.box.minY - boxCollider.box.height / 2);
        }

        public virtual void ExitCollision(CustomBoxCollider collider)
        {
            contactCollisions.Remove(collider.box);
            collider.isColliding = false;
        }

        public void ManageCollisions()
        {
            if (boxCollider)
            {
                foreach (var collider in Physics2d_Manager.instance.colliders)
                {
                    if (collider == boxCollider)
                        continue;

                    bool collision = CustomPhysics.CheckAABB(boxCollider.box, collider.box, out Vector2 normal);
                    if (collision)
                    {
                        Collision col = new Collision(collider, normal);
                        if (!contactCollisions.ContainsKey(collider.box))
                            CollisionWithBox(col);
                    }
                    else if (contactCollisions.ContainsKey(collider.box))
                        ExitCollision(collider);
                }
            }
        }

        public virtual void Update()
        {
            boxCollider.isColliding = IsColliding();
            contacts = contactCollisions.Values.ToList();
        }

        public virtual void FixedUpdate()
        {
            ManageCollisions();
            inAir = !CheckGround();
        }
    }
}
