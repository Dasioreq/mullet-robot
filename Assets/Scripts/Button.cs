using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class ButtonController : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
