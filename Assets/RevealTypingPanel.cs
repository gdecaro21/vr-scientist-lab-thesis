using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RevealTypingPanel : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("The button that appears after the voiceover. Starts non-interactable.")]
    public Button button;

    [Tooltip("The panel currently visible — will be hidden when the button is clicked.")]
    public GameObject voiceoverPanel;

    [Tooltip("The panel containing the input field and keyboard — will be shown when the button is clicked.")]
    public GameObject typingPanel;

    [Header("Audio gating")]
    [Tooltip("The AudioSource for this stop's voiceover. Button stays disabled until this AudioSource stops playing.")]
    public AudioSource voiceover;

    void Awake()
    {
        if (button != null)
        {
            button.interactable = false;
            button.onClick.AddListener(SwapPanels);
        }
        if (typingPanel != null) typingPanel.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(EnableWhenReady());
    }

    private IEnumerator EnableWhenReady()
    {
        if (voiceover != null)
        {
            yield return null;
            while (voiceover.isPlaying) yield return null;
        }
        if (button != null) button.interactable = true;
    }

    public void SwapPanels()
    {
        if (voiceoverPanel != null) voiceoverPanel.SetActive(false);
        if (typingPanel != null) typingPanel.SetActive(true);
    }
}