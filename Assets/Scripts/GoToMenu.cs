using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// @class GoToMenu
/// @brief Script for loading the main menu scene
public class GoToMenu : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
    }

    /// @brief Helper function called on Button click
    void TaskOnClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
