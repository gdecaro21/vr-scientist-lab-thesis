using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

[RequireComponent(typeof(TeleportationAnchor))]
public class StationAudio : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("Voiceover that plays when the player arrives at this station.")]
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;

    [Header("Manual button-reveal timing")]
    [Tooltip("If greater than 0, the onAudioFinished event fires after this many seconds " +
             "instead of using the clip's actual length. Set this to the length of your " +
             "audio file (in seconds) when you want to control the timing yourself. " +
             "Leave at 0 to use clip.length automatically.")]
    public float manualButtonDelay = 0f;

    [Header("Auto-play")]
    [Tooltip("Tick this on the FIRST station only — plays the clip at scene start.")]
    public bool playOnStart = false;

    [Header("Events")]
    [Tooltip("Fires when the voiceover finishes (or after manualButtonDelay if set).")]
    public UnityEvent onAudioFinished;

    private AudioSource audioSource;
    private TeleportationAnchor anchor;
    private TeleportationProvider provider;
    private Coroutine playRoutine;

    private static StationAudio s_pendingArrival;
    private static StationAudio s_lastPlayed;

    void Awake()
    {
        anchor = GetComponent<TeleportationAnchor>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        if (audioSource.isPlaying) audioSource.Stop();
    }

    void OnEnable()
    {
        provider = anchor != null ? anchor.teleportationProvider : null;
        if (provider == null)
        {
#if UNITY_2022_2_OR_NEWER
            provider = UnityEngine.Object.FindAnyObjectByType<TeleportationProvider>();
#else
            provider = FindObjectOfType<TeleportationProvider>();
#endif
        }
        if (provider != null) provider.locomotionEnded += OnLocomotionEnded;
    }

    void OnDisable()
    {
        if (provider != null) provider.locomotionEnded -= OnLocomotionEnded;
        if (s_pendingArrival == this) s_pendingArrival = null;
    }

    void Start()
    {
        if (playOnStart) PlayNow();
    }

    public void RegisterPendingArrival() { s_pendingArrival = this; }

    private void OnLocomotionEnded(LocomotionProvider _)
    {
        if (s_pendingArrival != this) return;
        s_pendingArrival = null;
        PlayNow();
    }

    public void PlayNow()
    {
        if (s_lastPlayed != null && s_lastPlayed != this && s_lastPlayed.audioSource != null)
            s_lastPlayed.audioSource.Stop();

        // If there's no clip AND no manual delay, just fire immediately.
        if (clip == null && manualButtonDelay <= 0f) { onAudioFinished?.Invoke(); return; }

        if (playRoutine != null) StopCoroutine(playRoutine);
        playRoutine = StartCoroutine(PlayAndNotify());
    }

    private IEnumerator PlayAndNotify()
    {
        if (clip != null) audioSource.PlayOneShot(clip, volume);
        s_lastPlayed = this;

        float wait = manualButtonDelay > 0f
            ? manualButtonDelay
            : (clip != null ? clip.length : 0f);

        yield return new WaitForSeconds(wait);
        onAudioFinished?.Invoke();
        playRoutine = null;
    }
}