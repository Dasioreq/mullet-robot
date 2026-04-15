using UnityEngine;
using UnityEngine.UI;

/**
* @class Button2
* @brief Helper script for the main menu button to exit the game
*/
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
