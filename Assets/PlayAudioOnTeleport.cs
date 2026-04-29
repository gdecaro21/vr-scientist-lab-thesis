using UnityEngine;


[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor))]
public class PlayAudioOnTeleport : MonoBehaviour
{
    [Tooltip("Drag in a unique audio clip for each anchor")]
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    private AudioSource audioSource;
    private UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor anchor;

    private static AudioSource s_LastPlayedAnchorAudio;

    void Awake()
    {
        anchor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D so the player always hears it at full volume
    }

    void OnEnable()
    {
        anchor.teleporting.AddListener(OnTeleport);
    }

    void OnDisable()
    {
        anchor.teleporting.RemoveListener(OnTeleport);
    }

    private void OnTeleport(UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportingEventArgs args)
    {
        if (clip != null)
        {
            if (s_LastPlayedAnchorAudio != null && s_LastPlayedAnchorAudio != audioSource)
                s_LastPlayedAnchorAudio.Stop();
            audioSource.PlayOneShot(clip, volume);
            s_LastPlayedAnchorAudio = audioSource;
        }
    }
}