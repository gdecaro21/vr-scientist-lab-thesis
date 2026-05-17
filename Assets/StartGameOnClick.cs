using UnityEngine;
using TMPro;
using System.Collections;

public class StartGameOnClick : MonoBehaviour
{
    [Tooltip("How long the fade FROM black takes, in seconds.")]
    public float fadeDuration = 1.5f;

    [Tooltip("Seconds to wait after click before the fade starts.")]
    public float delayBeforeFade = 0.5f;

    [Tooltip("CanvasGroup on the full-screen black UI Image. Should start at alpha 1.")]
    public CanvasGroup blackScreen;

    [Tooltip("Optional. The Start button GameObject - will be hidden once the fade begins.")]
    public GameObject startButton;

    [Tooltip("Optional. Welcome text on the black screen - will be hidden once the fade begins.")]
    public GameObject welcomeText;

    // Hook this to your Start Button's OnClick() in the Inspector.
    public void OnStartClicked()
    {
        StartCoroutine(StartRoutine());
    }

    IEnumerator StartRoutine()
    {
        if (startButton != null) startButton.SetActive(false);
        if (welcomeText != null) welcomeText.SetActive(false);

        yield return new WaitForSeconds(delayBeforeFade);

        if (blackScreen != null)
        {
            float t = 0f;
            float startAlpha = blackScreen.alpha;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                blackScreen.alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
                yield return null;
            }
            blackScreen.alpha = 0f;
            blackScreen.gameObject.SetActive(false);
        }
    }
}