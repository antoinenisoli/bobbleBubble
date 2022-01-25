using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    AnimationScript anim;
    PlayerController player;

    private void Awake()
    {
        anim = GetComponent<AnimationScript>();
        player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (player.hasJumped)
            anim.SetCurrentAnim("Jump");
        else if (player.body.inAir && !player.hasJumped)
            anim.SetCurrentAnim("Air");
        else
            anim.SetCurrentAnim("Idle");

        if (Input.GetKeyDown(KeyCode.O))
            anim.PlayAnimOnce("Shoot");
    }
}
