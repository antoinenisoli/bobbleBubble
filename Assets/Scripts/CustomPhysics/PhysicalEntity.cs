using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class PhysicalEntity : MonoBehaviour
    {
        [Header(nameof(PhysicalEntity))]
        [SerializeField] protected CustomBoxCollider boxCollider;
        [SerializeField] protected Vector2 localVelocity;
        public bool inAir;
        public bool onWall;
        protected Dictionary<PhysicBox, Collision> contactCollisions = new Dictionary<PhysicBox, Collision>();
        protected Vector2 normal;
        protected CustomPhysicBody body;

        public virtual void Awake()
        {
            body = GetComponent<CustomPhysicBody>();
        }

        public virtual void Landing()
        {
            print(normal);
            inAir = false;
            localVelocity.y = 0;
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

                bool collision = CustomPhysics.CheckAABB(boxCollider.box, collider.box);
                if (collision)
                {
                    normal = CustomPhysics.GetNormal(boxCollider.box, collider.box);
                    if (normal.y != 0)
                        return true;
                }
            }

            return false;
        }

        public void ManageCollisions()
        {
            if (boxCollider)
            {
                boxCollider.box.velocity = localVelocity;
                foreach (var collider in CustomPhysics2d_Manager.instance.colliders)
                {
                    if (collider == boxCollider)
                        continue;

                    bool collision = CustomPhysics.CheckAABB(boxCollider.box, collider.box);
                    if (collision)
                    {
                        normal = CustomPhysics.GetNormal(boxCollider.box, collider.box);
                        if (!boxCollider.isColliding && inAir)
                            Landing();

                        Collision col = new Collision(collider.box, normal);
                        if (!contactCollisions.ContainsKey(collider.box))
                        {
                            contactCollisions.Add(col.box, col);
                            collider.isColliding = true;
                        }
                    }
                    else
                    {
                        if (contactCollisions.ContainsKey(collider.box))
                        {
                            normal = new Vector2();
                            contactCollisions.Remove(collider.box);
                            collider.isColliding = false;
                        }
                    }
                }
            }
        }

        public virtual void Update()
        {
            boxCollider.isColliding = IsColliding();
            ManageCollisions();
        }
    }
}
