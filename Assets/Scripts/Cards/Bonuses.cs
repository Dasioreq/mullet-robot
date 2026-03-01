using System.Collections;
using UnityEngine;
using static RandCards;
using UnityEngine.UI;

public class Bonuses : MonoBehaviour
{
    public RandCards.UpgradeData data;
    public RandCards manager;
    public void OnClick()
    {
        manager.ApplyUpgrade(data);
    }
    public void Setup(RandCards.UpgradeData newData, RandCards m)
    {
        data = newData;
        manager = m;
    }
}
