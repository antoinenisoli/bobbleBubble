using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] string startAnim = "Idle";
    [SerializeField] protected SpriteRenderer animRenderer;
    [SerializeField] protected CustomAnimation[] animations;
    Dictionary<string, CustomAnimation> customAnimations = new Dictionary<string, CustomAnimation>();
    CustomAnimation currentAnim = null;
    string previousAnim;
    bool done = true;

    private void Awake()
    {
        foreach (var item in animations)
        {
            item.Init();
            customAnimations.Add(item.name, item);
        }

        SetCurrentAnim(startAnim);
    }

    public CustomAnimation GetAnimation(string stateName)
    {
        return customAnimations[stateName];
    }

    public void SetCurrentAnim(string stateName)
    {
        if (!done)
            return;

        if (currentAnim != null)
        {
            if (stateName != previousAnim)
            {
                currentAnim = customAnimations[stateName];
                previousAnim = stateName;
            }
        }
        else
            currentAnim = GetAnimation(stateName);
    }

    public void PlayAnimOnce(string stateName, string nextState = "")
    {
        if (currentAnim != null)
            currentAnim.Stop();

        if (stateName != previousAnim)
        {
            done = false;
            StopAllCoroutines();
            currentAnim = GetAnimation(stateName);
            StartCoroutine(Anim(nextState));
            previousAnim = stateName;
        }
    }

    IEnumerator Anim(string nextState = "")
    {
        while (!currentAnim.done)
            yield return null;

        currentAnim.Stop();
        done = true;
        if (!string.IsNullOrEmpty(nextState))
            currentAnim = GetAnimation(nextState); 
    }

    private void Update()
    {
        if (currentAnim != null)
        {
            currentAnim.Update();
            animRenderer.sprite = currentAnim.mainSprite;
        }
    }
}
