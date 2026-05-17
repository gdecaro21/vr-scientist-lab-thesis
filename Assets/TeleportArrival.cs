using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportArrival : MonoBehaviour
{
    [Header("Teleport")]
    [Tooltip("The TeleportationProvider component on your XR Origin / XR Rig.")]
    public TeleportationProvider teleportationProvider;

    [Tooltip("Empty GameObject placed exactly where you want the XR Origin base to land.")]
    public Transform destinationPoint;

    [Tooltip("The next stop's GameObject to activate when the player teleports.")]
    public GameObject nextStop;

    [Header("UI")]
    [Tooltip("The UI Button the player clicks. Will be set non-interactable until the voiceover (and optional action) are done.")]
    public Button button;

    [Header("Voiceover gating — pick ONE method per stop")]
    [Tooltip("METHOD 1 (Timer): set this to the length of your audio clip in seconds. The button appears after this many seconds. Leave at 0 to skip.")]
    public float voiceoverDuration = 0f;

    [Tooltip("METHOD 2 (AudioSource): drag in the AudioSource that plays this stop's VO. Button stays hidden until it stops playing. Leave empty if using Method 1.")]
    public AudioSource voiceover;

    [Tooltip("METHOD 3 (Timeline Signal): have a Signal Emitter on the Timeline call MarkVoiceoverDone() at the moment the VO ends.")]
    public bool useSignalInsteadOfAudioSource = false;

    [Header("Action gating (optional)")]
    [Tooltip("If true, the button ALSO waits until MarkActionComplete() is called.")]
    public bool requireAction = false;

    private bool voiceoverDone = false;
    private bool actionComplete = false;

    void Awake()
    {
        if (button != null)
        {
            button.gameObject.SetActive(false);
            button.onClick.AddListener(Teleport);
        }
    }

    void OnEnable()
    {
        StartCoroutine(EnableWhenReady());
    }

    private IEnumerator EnableWhenReady()
    {
        if (voiceoverDuration > 0f)
        {
            yield return new WaitForSeconds(voiceoverDuration);
        }
        else if (useSignalInsteadOfAudioSource)
        {
            while (!voiceoverDone) yield return null;
        }
        else if (voiceover != null)
        {
            yield return null;
            while (!voiceover.isPlaying) yield return null;
            while (voiceover.isPlaying) yield return null;
        }

        while (requireAction && !actionComplete) yield return null;

        if (button != null) button.gameObject.SetActive(true);
    }

    public void MarkVoiceoverDone()
    {
        voiceoverDone = true;
    }

    public void MarkActionComplete()
    {
        actionComplete = true;
    }

    public void Teleport()
    {
        if (teleportationProvider == null || destinationPoint == null) return;

        var request = new TeleportRequest
        {
            destinationPosition = destinationPoint.position,
            destinationRotation = destinationPoint.rotation,
            matchOrientation = MatchOrientation.WorldSpaceUp,
        };
        if (nextStop != null) nextStop.SetActive(true);
        teleportationProvider.QueueTeleportRequest(request);
    }
}