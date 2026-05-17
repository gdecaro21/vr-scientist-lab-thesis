using UnityEngine;
using UnityEngine.UI;

public class ShowKeyboard : MonoBehaviour
{
    public Button P5Button;
    public GameObject keyboard;
    public GameObject nextButton;

    void Start()
    {
        Button btn = P5Button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("P5 Button clicked — activating keyboard and next button");
        keyboard.SetActive(true);
        nextButton.SetActive(true);
    }
}