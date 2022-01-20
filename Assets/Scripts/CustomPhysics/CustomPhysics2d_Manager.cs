using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomPhysics2d_Manager : MonoBehaviour
{
    public static CustomPhysics2d_Manager instance;
    public Vector3 baseGravity = Vector3.up * -9.81f;
    public List<CustomBoxCollider> colliders;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        colliders = FindObjectsOfType<CustomBoxCollider>().ToList();
    }
}
