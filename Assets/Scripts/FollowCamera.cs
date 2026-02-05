using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    GameObject cam;

    void FixedUpdate()
    {
        transform.rotation = cam.transform.rotation;
    }
}
