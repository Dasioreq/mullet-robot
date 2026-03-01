using System;
using TMPro;
using UnityEngine;

public class RandCards : MonoBehaviour
{
    public Bonuses[] Btn;
    public enum UpgradeType{ speed, jump, dashing }
    public enum UpgradeTier { tier1, tier2, tier3 }
    public struct UpgradeData
    {
        public UpgradeType type;
        public UpgradeTier tier;
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
                UpgradeData randomUpgrade = GetRandomUpgrade();
                bonusScript.Setup(randomUpgrade, this);
            }
        }
    }
    private UpgradeData GetRandomUpgrade()
    {
        var values = Enum.GetValues(typeof(UpgradeType));
        UpgradeType randomType = (UpgradeType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        UpgradeTier selectedTier;
        float multiplier;

        int roll = UnityEngine.Random.Range(0, 101);

        if (roll <= 60)
        {
            selectedTier = UpgradeTier.tier1;
            multiplier = 0.05f;
        }
        else if (roll <= 90)
        {
            selectedTier = UpgradeTier.tier2;
            multiplier = 0.10f;
        }
        else
        {
            selectedTier = UpgradeTier.tier3;
            multiplier = 0.15f;
        }

        return new UpgradeData { type = randomType, tier = selectedTier, multiplier = multiplier };
    }
    public void ApplyUpgrade(UpgradeData upgr)
    {
        switch (upgr.type)
        {
            case UpgradeType.speed:
                Debug.Log("Type: " + upgr.type + " | Tier: " + upgr.tier + " | Bonus: " + upgr.multiplier);
                break;
            case UpgradeType.jump:
                Debug.Log("Type: " + upgr.type + " | Tier: " + upgr.tier + " | Bonus: " + upgr.multiplier);
                break;
            case UpgradeType.dashing:
                Debug.Log("Type: " + upgr.type + " | Tier: " + upgr.tier + " | Bonus: " + upgr.multiplier);
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
