using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using static RandCards;
using static UnityEngine.Analytics.IAnalytic;
using Unity.VisualScripting;

public class RandCards : MonoBehaviour
{

    public Bonuses[] Btn;
    public List<Icons> Icon;
    public MovementHandler player;
    public PlayerDamage playerDmg;

    public EquipWeapon curWeap;
    private List<int> weaponBases = new List<int> {0,3,6};
    private int[] weaponProgress = new int[] {0,0,0};
    private string[] weaponNames = {"A","B","C"};

    public enum UpgradeType{speed, jump, dashing, health, dashingTime, newWeapon, upgradeWeapon, extraA, extraB, extraC}
    private Dictionary<UpgradeType, float> currentMultipliers = new Dictionary<UpgradeType, float>();
    private float startMultiplier = 1.10f;
    private List<UpgradeType> rareUpgrades = new List<UpgradeType>(){UpgradeType.extraA, UpgradeType.extraB, UpgradeType.extraC};
    private List<UpgradeType> usedRareUpgrades = new List<UpgradeType>();

    private void Awake()
    {
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            if (type == UpgradeType.upgradeWeapon) continue;
            currentMultipliers[type] = startMultiplier;
        }
    }

    public struct UpgradeData
    {
        public UpgradeType type;
        public float multiplier;
        public int weaponID;
        public string weaponName;
        public bool isUpgrade;
    }
    public void RCards(GameObject[] spawnedButtons)
    {
        var available = Enum.GetValues(typeof(UpgradeType))
        .Cast<UpgradeType>().Where(t => currentMultipliers.ContainsKey(t) && currentMultipliers[t] < 1.5f && !usedRareUpgrades.Contains(t)).ToList();

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

            if (available.Count == 0)
            {
                Destroy(btnObj);
                continue;
            }

            Bonuses bonusScript = btnObj.GetComponent<Bonuses>();
            if (bonusScript != null)
            {

                UpgradeType randomType;

                float randomIndex = UnityEngine.Random.Range(0f, 100f);
                bool found = false;
                randomType = available[0];

                var currentRares = available.Where(t => rareUpgrades.Contains(t)).ToList();
                var currentCommons = available.Where(t => !rareUpgrades.Contains(t)).ToList();

                for (int i = 0; i < currentRares.Count; i++)
                {
                    if (randomIndex <= (i + 1) * 3f && randomIndex > i * 3f)
                    {
                        randomType = currentRares[i];
                        found = true;
                        break;
                    }
                }
                if(!found)
                {
                    var pool = currentCommons.Count > 0 ? currentCommons : available;
                    randomType = pool[UnityEngine.Random.Range(0, pool.Count)];
                }

                available.Remove(randomType);

                UpgradeData upgrade = new UpgradeData
                {
                    type = randomType,
                    multiplier = currentMultipliers[randomType]
                };

                Icons ui = null;

                if (randomType == UpgradeType.newWeapon)
                {
                    var result = RandNewWeapon();
                    upgrade.weaponID = result.finalId;
                    upgrade.isUpgrade = result.isUpgrade;
                    upgrade.weaponName = weaponNames[upgrade.weaponID / 3];

                    ui = Icon.Find(x => x.type == (upgrade.isUpgrade ? UpgradeType.upgradeWeapon : UpgradeType.newWeapon));
                }
                else
                {
                    ui = Icon.Find(x => x.type == upgrade.type);
                }

                bonusScript.Setup(upgrade, this, ui);
            }
        }

        }
    public void ApplyUpgrade(UpgradeData upgr)
    {
        float multiplierUpgrade = 0;
        if (upgr.type != UpgradeType.newWeapon)
        {
            multiplierUpgrade = (currentMultipliers[upgr.type] + 0.05f > 1.2f) ? 0.02f : 0.05f;
            currentMultipliers[upgr.type] += multiplierUpgrade;
        }

        switch (upgr.type)
        {
            case UpgradeType.speed:
                player.acceleration *= (1 + multiplierUpgrade);
                player.maxVelocity *= (1 + multiplierUpgrade);
                break;
            case UpgradeType.jump:
                player.jumpHeight *= (1 + multiplierUpgrade);
                break;
            case UpgradeType.dashing:
                player.dashForce *= (1 + multiplierUpgrade);
                break;
            case UpgradeType.health:
                playerDmg.maxLifeTime *= (1 + multiplierUpgrade);
                break;
            case UpgradeType.dashingTime:
                player.dashCooldownTime *= (1 + multiplierUpgrade);
                break;
            case UpgradeType.newWeapon:
                int family = upgr.weaponID / 3;
                weaponProgress[family] = upgr.weaponID % 3;
                curWeap.Equip(upgr.weaponID);
                break;
            case UpgradeType.extraA:
                Debug.Log("Extra A");
                break;
            case UpgradeType.extraB:
                Debug.Log("Extra B");
                break;
            case UpgradeType.extraC:
                Debug.Log("Extra C");
                break;
            default:
                Debug.Log("Different option");
                break;
        }

        if (rareUpgrades.Contains(upgr.type))
        {
            usedRareUpgrades.Add(upgr.type);
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
    public (int finalId, bool isUpgrade) RandNewWeapon()
    {
        int randomListIndex = UnityEngine.Random.Range(0, weaponBases.Count);
        int drawnBaseId = weaponBases[randomListIndex];
        int playerFamilyIndex = curWeap.currentWeapon / 3;

        bool isUpgrade = (drawnBaseId / 3 == playerFamilyIndex);

        if (isUpgrade)
        {
            if (weaponProgress[randomListIndex] < 2)
            {
                int upgradedId = drawnBaseId + weaponProgress[randomListIndex] + 1;
                return (upgradedId, true);
            }
            else
            {
                return RandNewWeapon();
            }
        }

        int newId = drawnBaseId + weaponProgress[randomListIndex];
        return (newId, false);
    }
}
