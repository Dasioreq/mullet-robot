using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

/**
* @class ButtonController
* @brief Helper script for the main menu button to load the main game scene
*/
public class ButtonController : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
    }

    /// @brief Helper function called on Button click
    void TaskOnClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
