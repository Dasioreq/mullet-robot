using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSController : MonoBehaviour
{
    public GameObject fpsObject;      
    public Button fpsButton;         
    public Sprite fpsOnSprite;
    public Sprite fpsOffSprite;
    public TMP_Text buttonTMPText;    
    public Color textONColor = Color.black;
    public Color textOffColor = Color.white;

    private Image buttonImage;

    void Start()
    {
        fpsButton.onClick.AddListener(ToggleFPS);
    }

    public void ToggleFPS()
    {
        if (fpsObject != null)
            fpsObject.SetActive(!fpsObject.activeSelf);
        if (fpsObject.activeSelf)
        {
            fpsButton.gameObject.GetComponent<Image>().sprite = fpsOnSprite;
            buttonTMPText.gameObject.GetComponent<TMP_Text>().color = textONColor;
        }
        if (fpsObject.activeSelf == false)
        {
            fpsButton.gameObject.GetComponent<Image>().sprite = fpsOffSprite;
            buttonTMPText.gameObject.GetComponent<TMP_Text>().color = textOffColor;
        }
    }

}
