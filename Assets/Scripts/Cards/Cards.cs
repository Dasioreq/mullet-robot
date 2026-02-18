using UnityEngine;

public class Cards : MonoBehaviour
{
    public GameObject CardPanel;
    public GameObject Player;
    public GameObject Camera;
    public GameObject weapon;

    private void ShowCards()
    {
        Camera.GetComponent<CameraContoller>().enabled = false;
        Player.GetComponent<MovementHandler>().rb.linearVelocity = Vector3.zero;
        Player.GetComponent<MovementHandler>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CardPanel.SetActive(true);
        weapon.GetComponent<Animator>().enabled = false;
        weapon.GetComponent<AudioSource>().enabled = false;
        weapon.GetComponent<gun>().enabled = false;
    }

    public void CloseWin()
    {
        Camera.GetComponent<CameraContoller>().enabled = true;
        Player.GetComponent<MovementHandler>().enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        CardPanel.SetActive(false);
        weapon.GetComponent<Animator>().enabled = true;
        weapon.GetComponent<AudioSource>().enabled = true;
        weapon.GetComponent<gun>().enabled = true;
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
