using System.Collections;
using UnityEngine;
using UnityEngine.Events;  // For Unity events

public class CameraCutEffect : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;  // CanvasGroup for the fade image
    public UnityEvent onFadeInStart;     // Unity event for fade-in start
    public UnityEvent onFadeInFinish;    // Unity event for fade-in finish
    public UnityEvent onFadeOutStart;    // Unity event for fade-out start
    public UnityEvent onFadeOutFinish;   // Unity event for fade-out finish

    private void Start()
    {
        // Make sure the CanvasGroup starts as fully transparent
        fadeCanvasGroup.alpha = 0f;
    }

    public void FadeIn(float duration)
    {
        onFadeInStart.Invoke();  // Trigger fade-in start event
        StartCoroutine(FadeInCoroutine(duration));
    }

    private IEnumerator FadeInCoroutine(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;  // Ensure it's fully visible
        onFadeInFinish.Invoke();  // Trigger fade-in finish event
    }

    public void FadeOut(float duration)
    {
        onFadeOutStart.Invoke();  // Trigger fade-out start event
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / duration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;  // Ensure it's fully invisible
        onFadeOutFinish.Invoke();  // Trigger fade-out finish event
    }
}