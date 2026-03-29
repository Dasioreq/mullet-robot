using UnityEngine;
using static RandCards;
using static GameController;

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
        gameController.SetGameState(GameState.UpgradeSelection);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CardPanel.SetActive(true);

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CardPanel.SetActive(false);
        gameController.SetGameState(GameState.Normal);
        foreach (var btn in CardPanel.GetComponentsInChildren<Transform>())
        {
            if (btn.gameObject != null && btn.gameObject != CardPanel)
                Destroy(btn.gameObject);
        }
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
