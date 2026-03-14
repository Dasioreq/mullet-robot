using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using static RandCards;
using static UnityEngine.Analytics.IAnalytic;

public class RandCards : MonoBehaviour
{
    public Bonuses[] Btn;
    public List<Icons> Icon;
    public GameObject noMoreUpgradesWindow;

    public enum UpgradeType{ speed, jump, dashing, newWeapon }
    private Dictionary<UpgradeType, float> currentMultipliers = new Dictionary<UpgradeType, float>();
    private float startMultiplier = 1.10f;

    private void Awake()
    {
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            currentMultipliers[type] = startMultiplier;
        }
    }

    public struct UpgradeData
    {
        public UpgradeType type;
        public float multiplier;
    }
    public void RCards(GameObject[] spawnedButtons)
    {
        var available = Enum.GetValues(typeof(UpgradeType))
        .Cast<UpgradeType>()
        .Where(t => currentMultipliers[t] < 1.5f)
        .ToList();
        if (available.Count == 0)
        {
            GetComponent<Cards>().CloseWin();
            Debug.Log("It's over");
            foreach (var btn in spawnedButtons) if (btn != null) Destroy(btn);
            return;
        }

        if (spawnedButtons.Length >= 4)
        {
            spawnedButtons[0].transform.localPosition = new Vector3(600f, 0f, 0f);
            spawnedButtons[1].transform.localPosition = new Vector3(200f, 0f, 0f);
            spawnedButtons[2].transform.localPosition = new Vector3(-200f, 0f, 0f);
            spawnedButtons[3].transform.localPosition = new Vector3(-600f, 0f, 0f);
        }

        var values = Enum.GetValues(typeof(UpgradeType));

        foreach (GameObject btnObj in spawnedButtons)
        {
            if (btnObj == null) continue;
            Bonuses bonusScript = btnObj.GetComponent<Bonuses>();

            if (bonusScript != null)
            {
                UpgradeType randomType = available[UnityEngine.Random.Range(0, available.Count)];
                UpgradeData upgrade = new UpgradeData { type = randomType, multiplier = currentMultipliers[randomType] };
                Icons ui = Icon.Find(x => x.type == upgrade.type);
                bonusScript.Setup(upgrade, this, ui);
            }
        }
    }
    public void ApplyUpgrade(UpgradeData upgr)
    {
        float multiplierUpgrade = (currentMultipliers[upgr.type] + 0.05f > 1.5f) ? 0.01f : 0.05f;
        currentMultipliers[upgr.type] += multiplierUpgrade;
        switch (upgr.type)
        {
            case UpgradeType.speed:
                if(currentMultipliers[UpgradeType.speed] <= 1.5f)
                {
                    Debug.Log("Type: " + upgr.type + " | Bonus: " + upgr.multiplier);
                }
                else
                {
                    Debug.Log("Too fast");
                }
                    break;
            case UpgradeType.jump:
                if (currentMultipliers[UpgradeType.jump] <= 1.5f)
                {
                    Debug.Log("Type: " + upgr.type + " | Bonus: " + upgr.multiplier);
                }
                else
                {
                    Debug.Log("Too high");
                }
                break;
            case UpgradeType.dashing:
                if (currentMultipliers[UpgradeType.dashing] <= 1.5f)
                {
                    Debug.Log("Type: " + upgr.type + " | Bonus: " + upgr.multiplier);
                }
                else
                {
                    Debug.Log("Too good");
                }
                break;
            default: 
                Debug.Log("Different option");
                break;

        }

        GetComponent<Cards>().CloseWin();
        foreach (var b in Btn) 
        { 
            if (b != null)
            {
                Destroy(b.gameObject); 
            }
        }

    }
}
