using TMPro;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;

/// @class FpsShow
/// @brief Script for calculating the FPS
public class FpsShow : MonoBehaviour
{
    public TMP_Text text;
    [SerializeField] float wait;
    void Start()
    {
        StartCoroutine(fps());
        if (text != null)
        {
            text.alignment = TextAlignmentOptions.Right;
        }
    }

    /// @brief Coroutine that calculates the FPS every second
    public  IEnumerator fps()
    {
        while(true)
        {
            text.text = math.round((1 / Time.unscaledDeltaTime)).ToString();
            yield return new WaitForSeconds(wait);
        }
    }
}
