using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static RandCards;
using static UnityEngine.Analytics.IAnalytic;

public class RandCards : MonoBehaviour
{
    public Bonuses[] Btn;
    public List<Icons> Icon;
    public enum UpgradeType{ speed, jump, dashing }
    private Dictionary<UpgradeType, float> currentMultipliers = new Dictionary<UpgradeType, float>();
    private float startMultiplier = 0.10f;

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

        if (spawnedButtons.Length >= 4)
        {
            spawnedButtons[0].transform.localPosition = new Vector3(-400f, 200f, 0f);
            spawnedButtons[1].transform.localPosition = new Vector3(400f, 200f, 0f);
            spawnedButtons[2].transform.localPosition = new Vector3(-400f, -200f, 0f);
            spawnedButtons[3].transform.localPosition = new Vector3(400f, -200f, 0f);
        }

        var values = Enum.GetValues(typeof(UpgradeType));

        foreach (GameObject btnObj in spawnedButtons)
        {
            if (btnObj == null) continue;
            Bonuses bonusScript = btnObj.GetComponent<Bonuses>();

            if (bonusScript != null)
            {
                UpgradeData upgrade = GetRandomUpgrade();
                Icons ui = Icon.Find(x => x.type == upgrade.type);
                bonusScript.Setup(upgrade, this, ui);
            }
        }
    }

    private UpgradeData GetRandomUpgrade()
    {
        var values = Enum.GetValues(typeof(UpgradeType));
        UpgradeType randomType = (UpgradeType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        float multiplier = currentMultipliers[randomType];

        return new UpgradeData { type = randomType, multiplier = multiplier };
    }

    public void ApplyUpgrade(UpgradeData upgr)
    {
        float multiplierUpgrade = (currentMultipliers[upgr.type] >= 0.20f) ? 0.01f : 0.05f;
        currentMultipliers[upgr.type] += multiplierUpgrade;
        switch (upgr.type)
        {
            case UpgradeType.speed:
                if(currentMultipliers[UpgradeType.speed] < 1.5f)
                {
                    Debug.Log("Type: " + upgr.type + " | Bonus: " + upgr.multiplier);
                }
                else
                {
                    Debug.Log("Too fast");
                }
                    break;
            case UpgradeType.jump:
                if (currentMultipliers[UpgradeType.jump] < 1.5f)
                {
                    Debug.Log("Type: " + upgr.type + " | Bonus: " + upgr.multiplier);
                }
                else
                {
                    Debug.Log("Too high");
                }
                break;
            case UpgradeType.dashing:
                if (currentMultipliers[UpgradeType.dashing] < 1.5f)
                {
                    Debug.Log("Type: " + upgr.type + " | Bonus: " + upgr.multiplier);
                }
                else
                {
                    Debug.Log("Too good");
                }
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
