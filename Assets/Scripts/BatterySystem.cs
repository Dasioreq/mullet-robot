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
        var targetPos = basePosition - transform.parent.rotation * playerRb.linearVelocity * multiplier;

        Vector3 vel = Vector3.zero;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref vel, smoothTime);
    }
}