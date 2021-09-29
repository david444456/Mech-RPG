using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;

        // Start is called before the first frame update
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            
        }

        /// <summary>
        /// Immediately change the fade (alpha)
        /// </summary>
        public void FadeOutImmediate() {
            canvasGroup.alpha = 1;
        }

        /// <summary>
        /// It fade in "x" time
        /// </summary>
        /// <param name="time"> Time to duration the fade</param>
        /// <returns></returns>
        public Coroutine FadeOut(float time) {
            return Fade(1, time);
        }

        /// <summary>
        ///  It fade in "x" time
        /// </summary>
        /// <param name="time">Time to duration the fade</param>
        /// <returns></returns>
        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        /// <summary>
        /// Stop fade, and start fade
        /// </summary>
        /// <param name="target">If it's 0, is fadeIn, if it's 1, it gets dark</param>
        /// <param name="time">Time to duration of fading</param>
        /// <returns></returns>
        public Coroutine Fade(float target, float time) {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentActiveFade;

        }


        IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
            //StartCoroutine(FadeIn(3f));
        }

        
    }
}
