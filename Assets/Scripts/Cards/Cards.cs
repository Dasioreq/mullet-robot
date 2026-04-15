using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;
using static RandCards;

/// @class Cards
/// @brief Handles Upgrade card rendering and player input
public class Cards : MonoBehaviour
{
    public RandCards RandCards;
    public GameController gameController;
    public GameObject CardPanel;
    public GameObject Player;
    public GameObject Camera;
    public GameObject weapon;
    public PlayerDamage playerDmg;
    [SerializeField] private GameObject ButtonOrg;
    [SerializeField] private Transform cardPanel;
    private bool deathStatsHandled = false;
    GameObject[] Buttons = new GameObject[4];

    private bool savedKillBoost = false;
    private bool savedDamageBoost = false;

    /// @brief Opens the upgrade panel
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

    /// @brief Closes the upgrade panel
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

        if(playerDmg.GetLifeTime() <= 1f && !RandCards.damageBoostUsed)
        {
            RandCards.DamageBoost();
            RandCards.damageBoostUsed = true;
        }

        if (gameController.GetGameState() == GameState.DeathScreen && !deathStatsHandled)
        {
            RandCards.killBoostActive = false;
            RandCards.damageBoostActive = false;
            RandCards.damageBoostUsed = false;
            RandCards.usedRareUpgrades.Clear();
            SaveData();
        }
        else if (gameController.GetGameState() == GameState.Normal && deathStatsHandled)
        {
            RestoreData();
        }
    }

    /// @brief Called after the player respawns; Restores their weapons and upgrades based on whether or not they have No Weapon Loss or No Bonus Loss active
    public void RestoreData()
    {
        if (RandCards.noWeaponLoss == true)
        {
            RandCards.lastWeapID = GetComponent<RandCards>().curWeap.currentWeapon;
            GetComponent<RandCards>().curWeap.Equip(RandCards.lastWeapID);
            RandCards.noWeaponLoss = false;
        }
        else
        {
            GetComponent<RandCards>().curWeap.Equip(0);
            RandCards.ResetAllWeapons();
        }
        if (RandCards.noBonusLoss == false)
        {
            RandCards.ReturnNormalStats();
        }
        else if (RandCards.noBonusLoss == true)
        {
            if (savedKillBoost == true)
            {
                RandCards.killBoostActive = true;
            }
            if (savedDamageBoost == true)
            {
                RandCards.damageBoostActive = true;
            }
            RandCards.noBonusLoss = false;
        }

            deathStatsHandled = false;     
    }

    /// @brief Saves the player's upgrade data for future reuse
    void SaveData()
    {
        if (RandCards.noBonusLoss == true)
        {
            if (RandCards.killBoostActive == true)
            {
                savedKillBoost = true;
            }
            if (RandCards.damageBoostActive == true)
            {
                savedDamageBoost = true;
            }
        }
    }
}
