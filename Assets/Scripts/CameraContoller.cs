using UnityEngine;
using static GameController;

///
/// @class CameraController
/// @brief Script for controlling the player's camera with their mouse and modify the rotation from outside sources
/// 
public class CameraContoller : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Sensitivity")]
    [SerializeField] private float xSens;
    [SerializeField] private float ySens;

    float xRotation, yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = transform.rotation.eulerAngles.x;
        yRotation = transform.rotation.eulerAngles.y;

        Application.targetFrameRate = 240;
    }

    void Update()
    {
        if(gameController.GetGameState() != GameState.Normal)
            return;
        float xMouse = Input.GetAxisRaw("Mouse X") * xSens;
        float yMouse = Input.GetAxisRaw("Mouse Y") * ySens;

        yRotation += xMouse;
        xRotation -= yMouse;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        player.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    /// @brief Sets the camera rotation to face a given Quaternion, accounting for Gimbal Lock
    /// @param rotation The desired new rotation
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
        player.rotation = rotation;

        xRotation = rotation.eulerAngles.x;
        if (xRotation > 180) xRotation -= 360;
        
        yRotation = rotation.eulerAngles.y;
        if (yRotation > 180) yRotation -= 360;
    }
}
