using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text scoreText;

    private void Start()
    {
        EventManager.Instance.onScore.AddListener(UpdateScore);
    }

    void UpdateScore(int score)
    {
        scoreText.text = score + "";
    }
}
