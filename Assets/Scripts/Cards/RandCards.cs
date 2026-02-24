using System;
using UnityEngine;

public class RandCards : MonoBehaviour
{
    public enum UpgradeType
    {
        speed,
        jump
    }

    public static void RCards(GameObject b1, GameObject b2, GameObject b3, GameObject b4)
    {
        b4.transform.localPosition = new Vector3(-900f, 360f, 0f);
        b1.transform.localPosition = new Vector3(-900f, 120f, 0f);
        b2.transform.localPosition = new Vector3(-900f, -120f, 0f);
        b3.transform.localPosition = new Vector3(-900f, -360f, 0f);

        var values = Enum.GetValues(typeof(UpgradeType));
        UpgradeType Option1 = (UpgradeType)values.GetValue(UnityEngine.Random.Range(0,values.Length));
        UpgradeType Option2 = (UpgradeType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        UpgradeType Option3 = (UpgradeType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        UpgradeType Option4 = (UpgradeType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        Debug.Log("Dzieje Sie");
    }

    public void ApplyUpgrade(UpgradeType uprg)
    {
        switch ((int)uprg)
        {
            case 0:
                break;
            case 1:
                break;
        }

    }

}
