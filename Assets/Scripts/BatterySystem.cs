using UnityEngine;

public class SmoothFollowWithOffset : MonoBehaviour
{
    [SerializeField] Rigidbody playerRb;
    [SerializeField] float multiplier;
    [SerializeField] float smoothTime;
    Vector3 basePosition;

    void Start()
    {
        basePosition = transform.localPosition;
    }

    void Update()
    {
        var targetPos = basePosition - Quaternion.Inverse(transform.parent.rotation) * playerRb.linearVelocity * multiplier;

        Vector3 vel = Vector3.zero;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, smoothTime * Time.deltaTime);
    }
}