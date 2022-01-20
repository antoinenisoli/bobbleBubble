using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    UnityPhysics_PlayerController player;
    BoxCollider2D box;

    private void Awake()
    {
        player = FindObjectOfType<UnityPhysics_PlayerController>();
        box = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        box.enabled = transform.position.y < player.feet.transform.position.y;
    }
}
