using UnityEngine;
public class Collectible : MonoBehaviour
{
    public GameObject onCollectEffect;
    public AudioClip onCollectSound;

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.parent != null && col.transform.parent.name == "machine_2")
        {
            if (onCollectEffect != null)
                Instantiate(onCollectEffect, transform.position, transform.rotation);
            if (onCollectSound != null)
                AudioSource.PlayClipAtPoint(onCollectSound, transform.position);

            Destroy(gameObject);

            if (ScanTracker.instance != null)
                ScanTracker.instance.RegisterScan();
        }
    }
}