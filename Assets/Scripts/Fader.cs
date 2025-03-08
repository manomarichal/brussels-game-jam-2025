using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour
{
    public Image fadeImage; 
   
    public IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
    }
}
