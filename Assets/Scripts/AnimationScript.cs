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
    CustomAnimation currentAnim;
    CustomAnimation previousAnim;

    private void Awake()
    {
        foreach (var item in animations)
        {
            item.Init();
            customAnimations.Add(item.name, item);
        }
    }

    private void Start()
    {
        SetCurrentAnim(startAnim);
    }

    public void SetCurrentAnim(string name)
    {
        if (previousAnim != null)
            return;

        if (currentAnim != null)
            currentAnim.Stop();

        currentAnim = customAnimations[name];
    }

    public void PlayAnimOnce(string name)
    {
        print("play anim " + name);
        StopAllCoroutines();
        previousAnim = currentAnim;
        currentAnim = customAnimations[name];
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        while (!currentAnim.done)
            yield return null;

        currentAnim.Stop();
        currentAnim = previousAnim;
        previousAnim = null;
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
