using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnableDisable : MonoBehaviour
{
    public GameObject CheckImage;
    public AudioSource audio;
    public bool isEnabled = true;

    public void ButtonClicked()
    {
        isEnabled = !isEnabled;
        CheckImage.SetActive(isEnabled);
        audio.Play();
    }
}