using TMPro;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEditor;
using static UnityEngine.Rendering.DebugUI;
using Unity.Mathematics;

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
    public  IEnumerator fps()
    {
        while(true)
        {
            text.text = math.round((1 / Time.unscaledDeltaTime)).ToString();
            yield return new WaitForSeconds(wait);
        }
    }
}
