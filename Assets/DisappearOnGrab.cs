// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using System.Collections;

// public class DisappearOnGrab : MonoBehaviour
// {
//     public float delay = 3f;
//     public AudioSource audioSource;

//     private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

//     void Awake()
//     {
//         grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
//         grabInteractable.selectEntered.AddListener(OnGrab);
//     }

//     void OnGrab(SelectEnterEventArgs args)
//     {
//         StartCoroutine(DisappearAfterDelay(args));
//     }

//     IEnumerator DisappearAfterDelay(SelectEnterEventArgs args)
//     {
//         yield return new WaitForSeconds(delay);
//         grabInteractable.interactionManager.CancelInteractorSelection(args.interactorObject);
//         gameObject.SetActive(false);
//     }

//     void OnDestroy()
//     {
//         grabInteractable.selectEntered.RemoveListener(OnGrab);
//     }

//     public void PlayAudio()
//     {
//         if (audioSource != null)
//         {
//             audioSource.PlayDelayed(delay);
//         }
//     }
// }

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class DisappearOnGrab : MonoBehaviour
{
    public float delay = 0.5f;
    public AudioSource audioSource;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError($"[DisappearOnGrab] No XRGrabInteractable on '{name}'. Script will do nothing.");
            return;
        }
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        StartCoroutine(DisappearAfterDelay(args));
    }

    IEnumerator DisappearAfterDelay(SelectEnterEventArgs args)
    {
        yield return new WaitForSeconds(delay);

        // 1) Release the grab cleanly so the interactor isn't holding a hidden object.
        if (grabInteractable != null && grabInteractable.interactionManager != null && args.interactorObject != null)
            grabInteractable.interactionManager.CancelInteractorSelection(args.interactorObject);

        // 2) Play the sound on a detached one-shot AudioSource so SetActive(false) below
        //    cannot stop it. This is the whole point of the fix.
        PlayDetachedSound();

        // 3) Hide the lab coat. The temp audio object lives in the scene root and survives.
        gameObject.SetActive(false);
    }

    void PlayDetachedSound()
    {
        if (audioSource == null)
        {
            Debug.LogWarning($"[DisappearOnGrab] '{name}' has no audioSource assigned. No sound will play.");
            return;
        }

        AudioClip clip = audioSource.clip;
        if (clip == null)
        {
            Debug.LogWarning($"[DisappearOnGrab] AudioSource on '{name}' has no clip. Assign the clip in the AudioSource's 'AudioClip' field (not just 'Resource').");
            return;
        }

        // Build a temporary, parent-less GameObject with its own AudioSource that
        // mirrors the original's settings, then auto-destroy it after the clip ends.
        GameObject temp = new GameObject($"OneShot_{clip.name}");
        temp.transform.position = transform.position;

        AudioSource src = temp.AddComponent<AudioSource>();
        src.clip                  = clip;
        src.volume                = audioSource.volume;
        src.pitch                 = audioSource.pitch;
        src.spatialBlend          = audioSource.spatialBlend;
        src.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
        src.minDistance           = audioSource.minDistance;
        src.maxDistance           = audioSource.maxDistance;
        src.rolloffMode           = audioSource.rolloffMode;
        src.dopplerLevel          = audioSource.dopplerLevel;
        src.bypassEffects         = audioSource.bypassEffects;
        src.bypassListenerEffects = audioSource.bypassListenerEffects;
        src.bypassReverbZones     = audioSource.bypassReverbZones;
        src.Play();

        float lifetime = clip.length / Mathf.Max(0.01f, Mathf.Abs(src.pitch)) + 0.1f;
        Destroy(temp, lifetime);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnGrab);
    }
}