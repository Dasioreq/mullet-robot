using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button2 : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Application.Quit();
    }
}
