using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class Bubble : PhysicalEntity
    {
        [SerializeField] Vector2 direction;
        [SerializeField] float force;

        public override void Awake()
        {
            base.Awake();
            Shoot(direction, force);
        }

        public override void EnterBoxCollision(CustomCollision2D col)
        {
            base.EnterBoxCollision(col);
            if (!CustomPhysics.CompareLayer(interactWith, col.collider.gameObject.layer))
                return;

            if (col.normal.y > 0)
                direction = Vector2.left;
            else if (col.normal.y < 0)
                direction = Vector2.right;
            else if (col.normal.x > 0)
                direction = Vector2.up;
            else if (col.normal.x < 0)
                direction = Vector2.down;

            print(col.collider);
            body.velocity = direction * force;
        }

        public void Shoot(Vector2 dir, float force)
        {
            this.force = force;
            direction = dir;
            body.velocity = direction * force;
        }
    }
}