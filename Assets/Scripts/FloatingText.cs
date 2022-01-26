using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    [SerializeField] float animDuration;
    [SerializeField] float offset = 2f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * offset);
    }

    private void Awake()
    {
        transform
            .DOMoveY(transform.position.y + 2f, animDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
