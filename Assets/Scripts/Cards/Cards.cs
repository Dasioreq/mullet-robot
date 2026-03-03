using UnityEngine;
using static RandCards;

public class Cards : MonoBehaviour
{
    public GameObject CardPanel;
    public GameObject Player;
    public GameObject Camera;
    public GameObject weapon;
    [SerializeField] private GameObject ButtonOrg;
    [SerializeField] private Transform cardPanel;
    GameObject[] Buttons = new GameObject[4];

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
        weapon.GetComponent<Gun>().enabled = false;

        for (int i = 0; i < 4; i++)
        {
            Buttons[i] = Instantiate(ButtonOrg, cardPanel); 
        }
        RandCards rc = GetComponent<RandCards>();
        Bonuses[] bScripts = new Bonuses[4];
        for (int i = 0; i < 4; i++) bScripts[i] = Buttons[i].GetComponent<Bonuses>();
        RandCards randCardsScript = GetComponent<RandCards>();
        randCardsScript.Btn = bScripts;

        if (rc != null)
        {
            rc.RCards(Buttons);
        }
        else
        {
            Debug.Log("No RandCards on object " + gameObject.name);
        }
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
        weapon.GetComponent<Gun>().enabled = true;
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
