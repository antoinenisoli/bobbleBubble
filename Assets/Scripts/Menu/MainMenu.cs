using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject logo;
    [SerializeField] float duration;
    [SerializeField] float yOffset = 10f;

    private void Awake()
    {
        Vector2 basePosition = logo.transform.position;
        logo.transform.position = basePosition + Vector2.up * yOffset;
        logo.transform.DOMove(basePosition, duration).SetEase(Ease.Linear);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
