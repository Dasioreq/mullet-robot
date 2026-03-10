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

        if (ic != null)
        {
            titleText.text = ic.displayName;
            iconImage.sprite = ic.icon;
            //iconImage.color = ic.themeColor;
            iconImage.color = Color.white;
        }

        valueText.text = $"+{(data.multiplier * 100):0}%";
    }
}
