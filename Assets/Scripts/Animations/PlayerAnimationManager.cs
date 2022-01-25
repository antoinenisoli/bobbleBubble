using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    AnimationScript anim;
    PlayerController player;

    private void Start()
    {
        anim = GetComponent<AnimationScript>();
        player = GetComponentInParent<PlayerController>();
    }

    public void Shoot()
    {
        anim.PlayAnimOnce("Shoot");
    }

    void Update()
    {
        if (player.hasJumped)
            anim.SetCurrentAnim("Jump");
        else if (player.body.inAir && !player.hasJumped)
            anim.SetCurrentAnim("Air");
        else
            anim.SetCurrentAnim("Idle");
    }
}
