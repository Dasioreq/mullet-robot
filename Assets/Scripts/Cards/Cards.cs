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
        Debug.Log("test");
        Camera.GetComponent<CameraContoller>().enabled = false;
        Player.GetComponent<MovementHandler>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CardPanel.SetActive(true);

    }
    private void Start()
    {
        CardPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            ShowCards();
        }
    }
}
