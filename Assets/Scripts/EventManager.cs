using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CustomPhysics2D;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public class BodyEvent : UnityEvent<CustomBoxCollider> { }
    public BodyEvent onNewBodyCreated = new BodyEvent();
    public BodyEvent onBodyRemove = new BodyEvent();

    public class IntEvent : UnityEvent<int> { }
    public IntEvent onScore = new IntEvent();


    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
