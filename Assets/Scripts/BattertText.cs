using UnityEngine;
using TMPro;

/**
* @class BattertText
* @brief Helper script that sets the UI Battery text to the player's remaining Lifetime
*/
public class BattertText : MonoBehaviour
{
    [SerializeField] private PlayerDamage player;
    [SerializeField] private TMP_Text numberText;
    void Start()
    {
        if (numberText != null)
        {
            numberText.alignment = TextAlignmentOptions.Center;
        }
    }
    void Update()
    {
        if (player == null || numberText == null) return;
        float hp = player.GetLifeTime();
        int hpInt = Mathf.RoundToInt(hp);
        numberText.text = hpInt.ToString();
    }
}
