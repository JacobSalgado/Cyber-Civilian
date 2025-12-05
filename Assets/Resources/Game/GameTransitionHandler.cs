// using UnityEngine;
// using UnityEngine.UI;

// public class GameTransitionHandler : MonoBehaviour
// {
//     public TransitionScreen fadeScreen;
    
//     public float fadeInTime = 0.2f;
//     public float fadeAwayTime = 0.2f;

//     private float timer = 0f;
//     private bool fadingIn = false;
//     private bool fadingAway = false;
//     private Image image;

//     void Start()
//     {
//         fadeImage.enabled = false;

//     }

//     void Update()
//     {
//         if (fadingIn && timer <= fadeInTime)
//         {
//             timer += Time.deltaTime;
//             float alpha = fadeImage.color.a - Time.deltaTime / fadeInTime;
//             ChangeSpriteAlpha(fadeImage, alpha);

//             if (timer > fadeInTime)
//             {
//                 timer = 0f;
//                 fadingIn = false;
//                 ChangeSpriteAlpha(fadeImage, 0f);
//             }
//         }

//         if (fadingAway && timer <= fadeAwayTime)
//         {
//             timer += Time.deltaTime;
//             float alpha = fadeImage.color.a - Time.deltaTime / fadeAwayTime;
//             ChangeSpriteAlpha(fadeImage, alpha);

//             if (timer > fadeAwayTime)
//             {
//                 timer = 0f;
//                 fadingAway= false;
//                 ChangeSpriteAlpha(fadeImage, 1f);
//             }
//         }
//     }

//     public void FadeInTransition()
//     {
//         fadeImage.enabled = true;
//         fadingIn = true;
//     }

//     public void FadeAwayTransition()
//     {
//         fadeImage.enabled = true;
//         fadingAway = true;
//     }

//     public void ChangeSpriteAlpha(SpriteRenderer sr, float new_alpha)
//     {
//         Color tempColor = sr.color;
//         tempColor.a = new_alpha;
//         sr.color = tempColor;
//     }
// }
