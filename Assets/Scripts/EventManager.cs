using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public UnityEvent onPlayerJump = new UnityEvent();
    public UnityEvent onPlayerLanding = new UnityEvent();

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
