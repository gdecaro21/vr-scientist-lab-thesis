// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.UI;

// public class HealthBarScript : MonoBehaviour
// {
//     public Slider slider;
//     public void SetMaxHealth(int health)
//     {
//         slider.maxValue = health;
//         slider.value = health;
//     }
//     public void SetHealth(int health)
//     {
//         slider.value = health;
//     }
// }

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    [Header("Reveal")]
    public GameObject revealButton;     // drag your button here in Inspector
    private bool buttonRevealed = false;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        if (!buttonRevealed && slider.value >= slider.maxValue)
        {
            RevealButton();
        }
    }

    public void RevealButton()
    {
        if (buttonRevealed) return;
        buttonRevealed = true;
        if (revealButton != null) revealButton.SetActive(true);
    }
}