using UnityEngine;

/**
* @class CameraAnimation
* @brief Helper script for the main menu camera to smoothly sway around
*/
public class CameraAnimation : MonoBehaviour
{
    public float swayAmountX = 0.05f;
    public float swayAmountY = 0.03f;
    public float swaySpeed = 0.5f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * swaySpeed) * swayAmountX;
        float offsetY = Mathf.Cos(Time.time * swaySpeed) * swayAmountY;

        transform.localPosition = startPosition + new Vector3(offsetX, offsetY, 0);
    }
}
