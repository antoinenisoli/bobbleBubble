using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPhysics2D;

public class BubbleAnimationManager : MonoBehaviour
{
    AnimationScript anim;

    public void Start()
    {
        anim = GetComponent<AnimationScript>();
        Shoot();
    }

    public void Shoot()
    {
        anim.PlayAnimOnce("Shoot", "Idle");
    }
}
