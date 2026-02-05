using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform camPosition;

    void Update()
    {
        transform.position = camPosition.position;
    }
}
