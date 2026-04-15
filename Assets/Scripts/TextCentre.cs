using TMPro;
using UnityEngine;

/// @class TextCentre
/// @brief dynamically centers a Text component. Penalized for being Bri`ish
public class TextCentre : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;
    void Start()
    {
        if (numberText != null)
        {
            numberText.alignment = TextAlignmentOptions.Center;
        }
    }
}
