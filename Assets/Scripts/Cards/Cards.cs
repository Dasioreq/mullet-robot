using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;
using static RandCards;

public class Cards : MonoBehaviour
{
    public RandCards RandCards;
    public GameController gameController;
    public GameObject CardPanel;
    public GameObject Player;
    public GameObject Camera;
    public GameObject weapon;
    [SerializeField] private GameObject ButtonOrg;
    [SerializeField] private Transform cardPanel;
    private bool deathStatsHandled = false;
    GameObject[] Buttons = new GameObject[4];

    public void ShowCards()
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

    public IEnumerator WaitAndShowCards()
    {
        yield return new WaitForSeconds(0.1f);
        ShowCards();
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

        if (gameController.GetGameState() == GameState.DeathScreen)
        {
            SaveData();
        }
        else if (gameController.GetGameState() == GameState.Normal)
        {
            RestoreData();
        }
    }

    void RestoreData()
    {
        if (deathStatsHandled)
        {
            if (RandCards.noWeaponLoss == true)
            {
                GetComponent<RandCards>().curWeap.Equip(RandCards.lastWeapID);
                RandCards.noWeaponLoss = false;
            }

            if (RandCards.noBonusLoss == true)
            {
                if (RandCards.savedUpgrades.Count > 0)
                {
                    RandCards.currentMultipliers = new Dictionary<RandCards.UpgradeType, float>(RandCards.savedUpgrades);
                    GetComponent<RandCards>().RestoreStats();
                    RandCards.savedUpgrades.Clear();
                    RandCards.noBonusLoss = false;
                }
            }
            deathStatsHandled = false;
        }
        
    }
   void SaveData()
    {
        if (!deathStatsHandled)
        {
            if (RandCards.noWeaponLoss == true)
            {
                RandCards.lastWeapID = GetComponent<RandCards>().curWeap.currentWeapon;
            }

            if (RandCards.noBonusLoss == true)
            {
                if (RandCards.savedUpgrades.Count == 0)
                {
                    RandCards.savedUpgrades = new Dictionary<RandCards.UpgradeType, float>(RandCards.currentMultipliers);
                }
            }
            else if (RandCards.noBonusLoss == false)
            {
                RandCards.ReturnNormalStats();
            }
            deathStatsHandled = true;
        }
    }
}
