using System;
using TMPro;
using UnityEngine;

public class RandCards : MonoBehaviour
{
    public Bonuses[] Btn;
    public enum UpgradeType
    {
        speed,
        jump
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
                UpgradeType option = (UpgradeType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
                bonusScript.Setup(option, this);
            }
            else
            {
                Debug.Log($"Missing Bonuses on {btnObj.name}");
            }
        }
    }

    public void ApplyUpgrade(UpgradeType uprg)
    {

        foreach (var b in Btn)
        {
            if (b != null)
            {
                b.GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
        }
        switch (uprg)
        {
            case UpgradeType.speed:
                Debug.Log("speed");
                break;
            case UpgradeType.jump:
                Debug.Log("jump");
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
