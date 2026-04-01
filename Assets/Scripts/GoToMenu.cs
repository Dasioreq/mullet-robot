using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToMenu : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
