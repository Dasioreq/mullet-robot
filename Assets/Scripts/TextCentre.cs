using TMPro;
using UnityEngine;


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
