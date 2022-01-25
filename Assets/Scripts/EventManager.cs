using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CustomPhysics2D;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public UnityEvent onPlayerJump = new UnityEvent();
    public UnityEvent onPlayerLanding = new UnityEvent();
    public class BodyEvent : UnityEvent<CustomBoxCollider> { }
    public BodyEvent onNewBodyCreated = new BodyEvent();
    public BodyEvent onBodyRemove = new BodyEvent();

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
