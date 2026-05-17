using UnityEngine;
using System.Collections;

public class EndGameOnCheck : MonoBehaviour
{
    [Tooltip("Seconds to wait after the button is clicked before the fade starts.")]
    public float delay = 2f;

    [Tooltip("How long the fade to black takes, in seconds.")]
    public float fadeDuration = 1f;

    [Tooltip("CanvasGroup on a full-screen black UI Image. Should start at alpha 0.")]
    public CanvasGroup blackScreen;

    // Hook this to your P8 'check' Button -> OnClick() in the Inspector.
    public void OnCheckClicked()
    {
        StartCoroutine(EndGameRoutine());
    }

    IEnumerator EndGameRoutine()
    {
        yield return new WaitForSeconds(delay);

        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                blackScreen.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }
            blackScreen.alpha = 1f;
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}