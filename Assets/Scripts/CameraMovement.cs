using UnityEngine;

/// @class CameraMovement
/// @brief Script that sets the player camera position to the player object
/// 
/// Parenting a camera to a Rigidbody often causes jittering, so a helper script like this is necessary.
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform camPosition;

    void Update()
    {
        transform.position = camPosition.position;
    }
}
