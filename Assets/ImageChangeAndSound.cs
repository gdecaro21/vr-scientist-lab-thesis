using UnityEngine;

public class ImageChangeAndSound : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource source;

    [Header("Image to destroy on trigger")]
    [Tooltip("Drag the 'Emails' GameObject here. It will be destroyed when the player enters this trigger, revealing whatever is behind it.")]
    public GameObject emailsObject;

    BoxCollider soundTrigger;

    void Awake()
    {
        if (source == null) source = GetComponent<AudioSource>();
        soundTrigger = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        source.Play();

        if (emailsObject != null)
        {
            Destroy(emailsObject);
        }
    }
}
