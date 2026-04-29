using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioOnAnchorReached : MonoBehaviour
{
    [Tooltip("Drag the Teleport Anchor (10) here")]
    public TeleportationAnchor targetAnchor;

    private AudioSource audioSource;
    private bool hasPlayed;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void OnEnable()
    {
        if (targetAnchor != null)
            targetAnchor.teleporting.AddListener(OnTeleport);
    }

    void OnDisable()
    {
        if (targetAnchor != null)
            targetAnchor.teleporting.RemoveListener(OnTeleport);
    }

    private void OnTeleport(TeleportingEventArgs args)
    {
        if (hasPlayed) return;
        if (audioSource.clip == null) return;

        audioSource.Play();
        hasPlayed = true;
    }
}
