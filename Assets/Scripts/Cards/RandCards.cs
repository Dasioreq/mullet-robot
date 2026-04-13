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
    public GameController gameController;
    public Gun gun;
    public EquipWeapon curWeap;
    public int lastWeapID;
    private List<int> weaponBases = new List<int> { 0, 3, 6 };
    public int[] weaponProgress = new int[] { 0, 0, 0 };
    private string[] weaponNames = { "Revolver", "Double-Barrel", "Ironfang" }; // Notatka od mergera: boze ale edgy nazwa
    public bool noWeaponLoss = false;

    public bool noBonusLoss = false;
    public Dictionary<UpgradeType, float> savedUpgrades = new Dictionary<UpgradeType, float>();

    private float normalVel = 0;
    private float normalAcc = 0;
    private Dictionary<UpgradeType, float> baseValues = new Dictionary<UpgradeType, float>();

    public bool killBoostActive = false;
    public bool damageBoostActive = false;

    public enum UpgradeType { speed, jump, dashing, health, dashingTime, newWeapon, upgradeWeapon, noWeaponLoss, noBonusLoss, killBoost, damageBoost }
    public Dictionary<UpgradeType, float> currentMultipliers = new Dictionary<UpgradeType, float>();
    private float startMultiplier = 1.1f;
    public List<UpgradeType> rareUpgrades = new List<UpgradeType>() { UpgradeType.noBonusLoss, UpgradeType.noWeaponLoss, UpgradeType.killBoost, UpgradeType.damageBoost };
    public List<UpgradeType> usedRareUpgrades = new List<UpgradeType>();

    private void Awake()
    {
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            if (type == UpgradeType.upgradeWeapon) continue;
            currentMultipliers[type] = startMultiplier;
        }

        baseValues[UpgradeType.jump] = player.jumpHeight;
        baseValues[UpgradeType.dashing] = player.dashForce;
        baseValues[UpgradeType.health] = playerDmg.maxLifeTime;
        baseValues[UpgradeType.dashingTime] = player.dashCooldownTime;
        normalAcc = player.acceleration;
        normalVel = player.maxVelocity;
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

        int currentID = curWeap.currentWeapon;
        bool isMaxLevel = (currentID + 1) % 3 == 0;
        var available = Enum.GetValues(typeof(UpgradeType))
        .Cast<UpgradeType>()
        .Where(t => t != UpgradeType.upgradeWeapon)
        .Where(t => t == UpgradeType.newWeapon || 
        (currentMultipliers.ContainsKey(t) && 
        currentMultipliers[t] < 1.5f &&!usedRareUpgrades.Contains(t))).ToList();


        if (available.Count == 0)
        {
            GetComponent<Cards>().CloseWin();
            foreach (var btn in spawnedButtons) if (btn != null) Destroy(btn);
            return;
        }


        if (spawnedButtons.Length >= 4)
        {
            spawnedButtons[0].transform.localPosition = new Vector3(720f, 0f, 0f);
            spawnedButtons[1].transform.localPosition = new Vector3(240f, 0f, 0f);
            spawnedButtons[2].transform.localPosition = new Vector3(-240f, 0f, 0f);
            spawnedButtons[3].transform.localPosition = new Vector3(-720f, 0f, 0f);
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
                if (!found)
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

                if (randomType == UpgradeType.newWeapon || randomType == UpgradeType.upgradeWeapon)
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
        switch (upgr.type)
        {
            case UpgradeType.speed:
                float currentM = currentMultipliers[UpgradeType.speed];
                player.acceleration = normalAcc * currentM;
                player.maxVelocity = normalVel * currentM;
                break;
            case UpgradeType.jump:
                float jumpM = currentMultipliers[UpgradeType.jump];
                player.jumpHeight = baseValues[UpgradeType.jump] * jumpM;
                break;
            case UpgradeType.dashing:
                float dashM = currentMultipliers[UpgradeType.dashing];
                player.dashForce = baseValues[UpgradeType.dashing] * dashM;
                break;
            case UpgradeType.health:
                float healthM = currentMultipliers[UpgradeType.health];
                playerDmg.maxLifeTime = baseValues[UpgradeType.health] * healthM;
                break;
            case UpgradeType.dashingTime:
                float dashTimeM = currentMultipliers[UpgradeType.dashingTime];
                player.dashCooldownTime = baseValues[UpgradeType.dashingTime] * dashTimeM;
                break;
            case UpgradeType.newWeapon:
                int family = upgr.weaponID / 3;
                weaponProgress[family] = upgr.weaponID % 3;
                curWeap.Equip(upgr.weaponID);
                break;
            case UpgradeType.noWeaponLoss:
                noWeaponLoss = true;
                break;
            case UpgradeType.noBonusLoss:
                noBonusLoss = true;
                break;
            case UpgradeType.damageBoost:
                damageBoostActive = true;
                break;
            case UpgradeType.killBoost:
                killBoostActive = true;
                break;
            default:
                Debug.Log("Different option");
                break;
        }
        if (upgr.type != UpgradeType.newWeapon)
        {
            multiplierUpgrade = (currentMultipliers[upgr.type] + 0.05f > 1.2f) ? 0.02f : 0.05f;
            currentMultipliers[upgr.type] += multiplierUpgrade;
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

    void ReturnNormalSpeed()
    {
        player.maxVelocity = normalVel;
        player.acceleration = normalAcc;
    }

    public void ReturnNormalStats()
    {
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            currentMultipliers[type] = startMultiplier;
        }
        player.maxVelocity = normalVel;
        player.acceleration = normalAcc;
        player.jumpHeight = baseValues[UpgradeType.jump];
        player.dashForce = baseValues[UpgradeType.dashing];
        playerDmg.maxLifeTime = baseValues[UpgradeType.health];
        player.dashCooldownTime = baseValues[UpgradeType.dashingTime];
    }

    public void RestoreStats()
    {
        player.maxVelocity = normalVel * currentMultipliers[UpgradeType.speed];
        player.acceleration = normalAcc * currentMultipliers[UpgradeType.speed];
        player.jumpHeight = baseValues[UpgradeType.jump] * currentMultipliers[UpgradeType.jump];
        player.dashForce = baseValues[UpgradeType.dashing] * currentMultipliers[UpgradeType.dashing];
        playerDmg.maxLifeTime = baseValues[UpgradeType.health] * currentMultipliers[UpgradeType.health];
        player.dashCooldownTime = baseValues[UpgradeType.dashingTime] * currentMultipliers[UpgradeType.dashingTime];
    }

    public void ResetAllWeapons()
    {
        for (int i = 0; i < weaponProgress.Length; i++) { weaponProgress[i] = 0; }
        if (usedRareUpgrades.Contains(UpgradeType.upgradeWeapon))
        {
            usedRareUpgrades.Remove(UpgradeType.upgradeWeapon);
        }
    }

    public void KillBoost()
    {
        if (killBoostActive == true)
        {
            float boostLevel = 1.2f;
            player.maxVelocity = normalVel * boostLevel;
            player.acceleration = normalAcc * boostLevel;
            Invoke("ReturnNormalSpeed", 1f);
        }
        
    }

    public void DamageBoost()
    {
        if (damageBoostActive == true)
        {
            gun.damageMultiplier = 5f;
        }    
    }
}
