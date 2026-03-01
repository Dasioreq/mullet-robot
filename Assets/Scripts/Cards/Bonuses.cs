using System.Collections;
using UnityEngine;
using static RandCards;
using UnityEngine.UI;

public class Bonuses : MonoBehaviour
{
    public UpgradeType assignedUpgrade;
    public RandCards manager;

    public void OnClick()
    {
        if (manager != null)
        {
            manager.ApplyUpgrade(assignedUpgrade);
        }
        else
        {
            Debug.LogError("Manager is null");
        }
    }

    public void Setup(UpgradeType newUpgrade, RandCards m)
    {
        assignedUpgrade = newUpgrade;
        manager = m;
    }
}

