using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] Transform rootTransform;
    [SerializeField] Transform boneTransform;
    [SerializeField] float maxAngle;
    [SerializeField] float aimSpeed;

    void Update()
    {
        Vector3 dirToPlayer = targetTransform.position - boneTransform.position;
        
        Vector3 limitedDir = Vector3.RotateTowards(
            rootTransform.rotation * rootTransform.forward, 
            dirToPlayer.normalized, 
            maxAngle * Mathf.Deg2Rad,
            0
        );

        transform.position = Vector3.Lerp(transform.position, boneTransform.position + limitedDir * dirToPlayer.magnitude, Time.deltaTime * aimSpeed);
    }
}