using UnityEngine;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerBody : MonoBehaviour
{
    [Tooltip("The Camera that represents the player's head (Main Camera under XR Origin). " +
             "If left empty, this script will find the XR Origin in the scene at runtime and " +
             "use its Camera.")]
    public Transform headCamera;

    [Tooltip("The XR Origin's root transform. Used to keep this body on the floor " +
             "(this body's Y matches the XR Origin's Y). If left empty, found at runtime.")]
    public Transform xrOriginRoot;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;            // Physics shouldn't push us; only this script moves us.
        rb.useGravity = false;            // No falling.
        rb.interpolation = RigidbodyInterpolation.None;

        capsule = GetComponent<CapsuleCollider>();
        capsule.isTrigger = false;        // We ENTER triggers; we are not one.

        // Auto-wire if the user didn't drag references in the Inspector.
        if (headCamera == null || xrOriginRoot == null)
        {
#if UNITY_2022_2_OR_NEWER
            var origin = Object.FindAnyObjectByType<XROrigin>();
#else
            var origin = Object.FindObjectOfType<XROrigin>();
#endif
            if (origin != null)
            {
                if (xrOriginRoot == null) xrOriginRoot = origin.transform;
                if (headCamera == null && origin.Camera != null) headCamera = origin.Camera.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (headCamera == null) return;

        // The player's head is at headCamera.position in world space.
        // We want this body to sit directly under the head, on the floor.
        Vector3 head = headCamera.position;
        float floorY = xrOriginRoot != null ? xrOriginRoot.position.y : 0f;
        transform.position = new Vector3(head.x, floorY, head.z);

        // Kinematic Rigidbodies fall asleep when stationary; sleeping bodies can miss
        // OnTriggerEnter. Keep this one awake.
        if (rb.IsSleeping()) rb.WakeUp();
    }
}