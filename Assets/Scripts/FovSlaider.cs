using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// @class FovSlaider
/// @brief Script for changing the player camera's FOV value based on the slider in the options menu
public class FovSlaider : MonoBehaviour
{
    public Camera cam;
    public TMP_Text text;
    void Start()
    {
        GetComponent<Slider>().value = Camera.VerticalToHorizontalFieldOfView(cam.fieldOfView, cam.aspect); 
    }
    /// @brief Changes the FOV
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
