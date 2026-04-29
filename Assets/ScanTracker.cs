using UnityEngine;
using System.Collections;

public class ScanTracker : MonoBehaviour
{
    public int totalObjects = 5;
    public int scannedCount = 0;
    public HealthBarScript healthBar;
    public static ScanTracker instance;

    void Start()
    {
        instance = this;
        scannedCount = 0;
        healthBar.SetMaxHealth(totalObjects);
        healthBar.SetHealth(0);
    }

    public void RegisterScan()
    {
        scannedCount++;
        StartCoroutine(DelayedUpdate());
    }

    IEnumerator DelayedUpdate()
    {
        yield return new WaitForSeconds(5f);
        healthBar.SetHealth(scannedCount);
    }
}