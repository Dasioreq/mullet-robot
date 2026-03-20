using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class FovSlaider : MonoBehaviour
{
    public Camera cam;
    public TMP_Text text;
    void Start()
    {
        GetComponent<Slider>().value = Camera.VerticalToHorizontalFieldOfView(cam.fieldOfView, cam.aspect); 
    }
    public void ChangeFOV(float value)
    {
        cam.fieldOfView = Camera.HorizontalToVerticalFieldOfView(value, cam.aspect);
        text.text = math.round(value).ToString();
    }
    void Update()
    {
        ChangeFOV(GetComponent<Slider>().value);
    }

}
