using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [SerializeField] float gravityScale = 1f;
    public Vector3 velocity;

    private void Update()
    {
        velocity = GravityManager.instance.baseGravity * gravityScale * Time.deltaTime;
        transform.position += velocity;
    }
}
