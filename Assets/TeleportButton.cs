using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportButton : MonoBehaviour
{
    [Header("Teleport")]
    [Tooltip("The TeleportationAnchor this button sends the player to (the NEXT stop).")]
    public TeleportationAnchor destinationAnchor;

    [Tooltip("The TeleportationProvider component on your XR Origin / XR Rig.")]
    public TeleportationProvider teleportationProvider;

    [Header("UI")]
    [Tooltip("The UI Button the player clicks. Will be set non-interactable until the voiceover (and optional action) are done.")]
    public Button button;

    [Header("Audio gating")]
    [Tooltip("The AudioSource for this stop's voiceover. The button stays disabled until this AudioSource is no longer playing.")]
    public AudioSource voiceover;

    [Header("Action gating (optional)")]
    [Tooltip("If true, the button ALSO waits until MarkActionComplete() is called (e.g. wired to a lab-coat XRGrabInteractable's Select Entered event).")]
    public bool requireAction = false;

    private bool actionComplete = false;

    void Awake()
    {
        if (button != null)
        {
            button.interactable = false;
            button.onClick.AddListener(Teleport);
        }
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

        while (requireAction && !actionComplete) yield return null;

        if (button != null) button.interactable = true;
    }

    public void MarkActionComplete()
    {
        actionComplete = true;
    }

    public void Teleport()
    {
        if (destinationAnchor == null || teleportationProvider == null) return;

        var request = new TeleportRequest
        {
            destinationPosition = destinationAnchor.transform.position,
            destinationRotation = destinationAnchor.transform.rotation,
            matchOrientation = MatchOrientation.WorldSpaceUp,
        };
        teleportationProvider.QueueTeleportRequest(request);
    }
}
