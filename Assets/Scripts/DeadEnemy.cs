using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeadEnemy : MonoBehaviour
{
    [SerializeField] float jumpForce = 18f;
    [SerializeField] float jumpDuration = 2f;
    [SerializeField] float endPosY = -25;

    private void Awake()
    {
        int random = Random.Range(-1, 2);
        transform
            .DOJump(new Vector3(transform.position.x + (random * 2), endPosY), jumpForce, 1, jumpDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
