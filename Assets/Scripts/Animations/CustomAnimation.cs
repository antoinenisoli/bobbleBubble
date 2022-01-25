using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomAnimation
{
    public string name = "Idle";
    public Sprite[] sprites;
    public float frameRate = 0.1f;
    public float timer;
    public int spriteIndex;
    public bool loop = true;
    public bool done = false;
    public Sprite mainSprite;

    public void Init()
    {
        mainSprite = sprites[0];
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > frameRate && loop)
        {
            timer = 0;
            spriteIndex++;
            mainSprite = sprites[spriteIndex % sprites.Length];
            if (spriteIndex % sprites.Length == 0)
                done = true;
        }
    }

    public void Stop()
    {
        done = false;
        timer = 0;
        spriteIndex = 0;
    }
}
