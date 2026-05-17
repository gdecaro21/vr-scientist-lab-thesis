using UnityEngine;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class StationButton : MonoBehaviour
{
    [Header("Destination")]
    [Tooltip("Where the button teleports the XR Origin to.")]
    public Transform destination;

    [SerializeField]
    TeleportationProvider teleportationProvider;

    [SerializeField]
    Button button;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (teleportationProvider == null)
        {
    #if UNITY_2022_2_OR_NEWER
            teleportationProvider = UnityEngine.Object.FindAnyObjectByType<TeleportationProvider>();
    #else
            teleportationProvider = FindObjectOfType<TeleportationProvider>();
    #endif
        }

        if (button == null)
        {
            Debug.LogWarning($"{nameof(StationButton)} on '{name}' could not find a {nameof(Button)} component.");
            return;
        }

        button.gameObject.SetActive(true);
        button.interactable = true;
        button.onClick.AddListener(Teleport);
    }

    public void Teleport()
    {
        Debug.Log($"[StationButton] Teleport fired on '{name}' destination='{(destination != null ? destination.name : "NULL")}'");

        if (destination == null)
        {
            Debug.LogWarning($"{nameof(StationButton)} on '{name}' has no destination assigned.");
            return;
        }

        if (teleportationProvider == null)
        {
    #if UNITY_2022_2_OR_NEWER
            teleportationProvider = UnityEngine.Object.FindAnyObjectByType<TeleportationProvider>();
    #else
            teleportationProvider = FindObjectOfType<TeleportationProvider>();
    #endif
        }

        if (teleportationProvider == null)
        {
            Debug.LogWarning($"{nameof(StationButton)} on '{name}' could not find a {nameof(TeleportationProvider)} in the scene.");
            return;
        }

        var request = new TeleportRequest
        {
            destinationPosition = GetAdjustedDestination(destination.position),
            destinationRotation = destination.rotation,
            matchOrientation = MatchOrientation.WorldSpaceUp,
        };
        teleportationProvider.QueueTeleportRequest(request);
    }

    private Vector3 GetAdjustedDestination(Vector3 anchorPosition)
    {
#if UNITY_2022_2_OR_NEWER
        var xrOrigin = UnityEngine.Object.FindAnyObjectByType<XROrigin>();
#else
        var xrOrigin = FindObjectOfType<XROrigin>();
#endif
        if (xrOrigin == null || xrOrigin.Camera == null) return anchorPosition;

        Vector3 cameraOffset = xrOrigin.Camera.transform.position - xrOrigin.transform.position;
        cameraOffset.y = 0f;
        return anchorPosition - cameraOffset;
    }
}