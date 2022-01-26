using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPhysics2D;

public class BubbleAnimationManager : MonoBehaviour
{
    AnimationScript anim;
    Bubble bubble;

    public void Awake()
    {
        anim = GetComponent<AnimationScript>();
        bubble = GetComponentInParent<Bubble>();
    }

    private void Update()
    {
        if (bubble.contains)
            anim.SetCurrentAnim("ContainsEnemy");
        else if (bubble.projectile)
            anim.SetCurrentAnim("Shoot");
        else
            anim.SetCurrentAnim("Idle");
    }
}
