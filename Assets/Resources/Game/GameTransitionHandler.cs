using System;
using System.Collections;
using UnityEngine;

public class GameTransitionHandler : MonoBehaviour
{
    public enum TransitionType
    {
        FADE_IN,
        FADE_OUT,
    }

    [SerializeField] private GameManager manager;
    public TransitionScreen fadeScreen;
    
    private float fadeInTime = 0.2f;
    private float fadeAwayTime = 0.2f;

    private float timer = 0f;
    private bool fadingIn = false;
    private bool fadingAway = false;

    void Update()
    { 
        if (fadingIn && timer <= fadeInTime)
        {
            timer += Time.deltaTime;
            float alpha = fadeScreen.image.color.a - Time.deltaTime / fadeInTime;
            fadeScreen.ChangeScreenAlpha(alpha);

            if (timer > fadeInTime)
            {
                timer = 0f;
                fadingIn = false;
                fadeScreen.ChangeScreenAlpha(0f);
                fadeScreen.SetSortingOrder(-1);
            }
        }

        if (fadingAway && timer <= fadeAwayTime)
        {
            timer += Time.deltaTime;
            float alpha = fadeScreen.image.color.a + Time.deltaTime / fadeInTime;
            fadeScreen.ChangeScreenAlpha(alpha);

            if (timer > fadeAwayTime)
            {
                timer = 0f;
                fadingAway= false;
                fadeScreen.ChangeScreenAlpha(1f);
                fadeScreen.SetSortingOrder(-1);
            }
        }
    }

    public void FadeInTransition()
    {
        fadingIn = true;
        fadeScreen.SetSortingOrder(1);
    }

    public void FadeAwayTransition()
    {
        fadingAway = true;
        fadeScreen.SetSortingOrder(1);
    }

    public IEnumerator Transition(TransitionType type, float duration)
    {
        fadeScreen.SetSortingOrder(1);

        switch (type)
        {
            case TransitionType.FADE_IN:
                fadeInTime = duration;
                fadingIn = true;
                yield return new WaitForSeconds(fadeInTime);

                break;

            case TransitionType.FADE_OUT:
                fadeAwayTime = duration;
                fadingAway = true;
                yield return new WaitForSeconds(fadeAwayTime);

                break;
        }
    }
}
