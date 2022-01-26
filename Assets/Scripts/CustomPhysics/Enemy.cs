using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomPhysics2D;

public class Enemy : PhysicalEntity
{
    public int scoreValue = 500;
    [SerializeField] float speed = 0.01f;
    [SerializeField] int direction = 1;

    public override void EnterBoxCollision(CustomCollision2D col)
    {
        base.EnterBoxCollision(col);
        if (CustomPhysics.CompareLayer(interactWith, col.collider.gameObject.layer) && col.normal.x != 0)
            direction *= -1;
    }

    void ManageGraphics()
    {
        sprRenderer.flipX = direction > 0;
    }

    public override void Update()
    {
        base.Update();
        ManageGraphics();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float velocity = speed * Time.fixedDeltaTime * direction;
        body.velocity.x = velocity;
    }
}
