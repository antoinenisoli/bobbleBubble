using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    [SerializeField] float animDuration;
    [SerializeField] float offset = 2f;
    TextMesh textMesh;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * offset);
    }

    public void Create(int score)
    {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = score + "";
        transform
            .DOMoveY(transform.position.y + 2f, animDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
