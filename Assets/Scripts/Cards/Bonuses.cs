using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// @class Bonuses
/// @brief Helper script for upgrade buttons
public class Bonuses : MonoBehaviour
{
    public RandCards.UpgradeData data;
    public RandCards manager;
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI valueText;
    public Icons[] weaponIcons;

    /// @brief You are NOT gonna believe what it does
    public void OnClick()
    {
        manager.ApplyUpgrade(data);
    }
    /// @brief Sets the icon, text etc. to the upgrade card
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

        if (data.type == RandCards.UpgradeType.newWeapon)
        {    
            iconImage.preserveAspect = true;
            titleText.text = weaponIcons[data.weaponID].displayName;
            iconImage.sprite = weaponIcons[data.weaponID].icon;
            valueText.text = "";
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
