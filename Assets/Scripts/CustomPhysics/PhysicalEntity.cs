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
        protected CustomPhysicBody body;

        public virtual void Awake()
        {
            body = GetComponent<CustomPhysicBody>();
        }

        public virtual void Landing()
        {
            if (inAir)
                body.velocity.y = 0;
        }

        public bool IsColliding()
        {
            return contactCollisions.Count > 0;
        }

        public bool CheckGround()
        {
            foreach (var collider in CustomPhysics2d_Manager.instance.colliders)
            {
                if (collider == boxCollider)
                    continue;

                bool collision = CustomPhysics.CheckAABB(boxCollider.box, collider.box, out Vector2 normal);
                if (collision)
                {
                    if (normal.y > 0)
                        return true;
                }
            }      

            /*foreach (var collision in contactCollisions.Values)
            {
                print(collision.normal);
                if (collision.normal.y > 0)
                    return true;
            }*/

            return false;
        }

        public void CollisionWithBox(Collision col)
        {
            if (!contactCollisions.ContainsKey(col.collider.box))
            {
                Landing();
                contactCollisions.Add(col.collider.box, col);
                col.collider.isColliding = true;
            }
        }

        public void ExitCollision(CustomBoxCollider collider)
        {
            if (contactCollisions.ContainsKey(collider.box))
            {
                contactCollisions.Remove(collider.box);
                collider.isColliding = false;
            }
        }

        public void ManageCollisions()
        {
            if (boxCollider)
            {
                foreach (var collider in CustomPhysics2d_Manager.instance.colliders)
                {
                    if (collider == boxCollider)
                        continue;

                    bool collision = CustomPhysics.CheckAABB(boxCollider.box, collider.box, out Vector2 normal);
                    if (collision)
                    {
                        Collision col = new Collision(collider, normal);
                        CollisionWithBox(col);
                    }
                    else
                        ExitCollision(collider);
                }
            }
        }

        public virtual void Update()
        {
            inAir = !CheckGround();
            boxCollider.isColliding = IsColliding();
            ManageCollisions();
            contacts = contactCollisions.Values.ToList();
        }
    }
}
