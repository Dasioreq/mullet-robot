using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RandCards;

public class Bonuses : MonoBehaviour
{
    public RandCards.UpgradeData data;
    public RandCards manager;
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI valueText;

    public void OnClick()
    {
        manager.ApplyUpgrade(data);
    }
    public void Setup(RandCards.UpgradeData newData, RandCards m, Icons ic)
    {
        data = newData;
        manager = m;
        bool isRare = manager.rareUpgrades.Contains(data.type);

        if (ic != null)
        {
            titleText.text = ic.displayName;
            iconImage.sprite = ic.icon;
            iconImage.color = ic.themeColor;
        }

        if (data.type == RandCards.UpgradeType.newWeapon || data.type == RandCards.UpgradeType.upgradeWeapon)
        {
            if (data.isUpgrade)
            {
                int level = (data.weaponID % 3) + 1;
                valueText.text = $"LVL {level}";
            }
            else
            {
                valueText.text = data.weaponName;
            }
        }
        else if (isRare) 
        {
            valueText.text = "";
        }
        else
        {
            valueText.text = $"+{((data.multiplier - 1.0f) * 100):0}%";
        }
    }
}
