using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomPhysics2D;

public class Enemy : PhysicalEntity
{
    public int scoreValue = 500;
    [SerializeField] float speed = 0.01f;
    Vector2 direction = Vector2.down;

    public override void EnterBoxCollision(CustomCollision2D col)
    {
        base.EnterBoxCollision(col);
        if (col.normal.y > 0)
            direction = Vector2.left;
        else if (col.normal.y < 0)
            direction = Vector2.right;
        else if (col.normal.x > 0)
            direction = Vector2.up;
        else if (col.normal.x < 0)
            direction = Vector2.down;
    }

    public override void ExitBoxCollision(CustomCollision2D col)
    {
        base.ExitBoxCollision(col);
        Vector2 normal = CustomPhysics.GetNormal(col.collider.box, boxCollider.box);
        if (normal.y > 0)
            direction = Vector2.down;
        else if (normal.y < 0)
            direction = Vector2.up;
        else if (normal.x > 0)
            direction = Vector2.left;
        else if (normal.x < 0)
            direction = Vector2.right;
    }

    public override void Update()
    {
        base.Update();
        Vector2 velocity = speed * Time.deltaTime * direction;
        //body.velocity = velocity;
    }
}
