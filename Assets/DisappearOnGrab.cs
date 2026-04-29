using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class DisappearOnGrab : MonoBehaviour
{
    public float delay = 3f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        StartCoroutine(DisappearAfterDelay(args));
    }

    IEnumerator DisappearAfterDelay(SelectEnterEventArgs args)
    {
        yield return new WaitForSeconds(delay);
        grabInteractable.interactionManager.CancelInteractorSelection(args.interactorObject);
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }
}