using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Cards : MonoBehaviour
{
    public GameObject CardPanel;
    public GameObject Player;
    public GameObject Camera;
    private void ShowCards()
    {
        Camera.GetComponent<CameraContoller>().enabled = false;
        Player.GetComponent<MovementHandler>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CardPanel.SetActive(true);

    }

    public void CloseWin()
    {
        Camera.GetComponent<CameraContoller>().enabled = true;
        Player.GetComponent<MovementHandler>().enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        CardPanel.SetActive(false);
    }

    void Start()
    {
        CardPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown("t") && CardPanel.activeSelf==false)
        {
            ShowCards();
        }
        else if (Input.GetKeyDown("t") && CardPanel.activeSelf == true)
        {
            CloseWin();
        }
    }
}
