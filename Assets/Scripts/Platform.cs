using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    PlayerController player;
    BoxCollider2D box;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        box = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        box.enabled = transform.position.y < player.feet.transform.position.y;
    }
}
