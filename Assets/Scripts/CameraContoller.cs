using UnityEngine;
using static GameController;

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

        Application.targetFrameRate = 480;
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

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
        player.rotation = rotation;

        xRotation = yRotation = 0;
    }
}
