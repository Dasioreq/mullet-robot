using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] string targetTransformName;
    [SerializeField] float range;
    [SerializeField] Transform rootTransform;
    [SerializeField] Transform boneTransform;
    [SerializeField] float maxAngle;
    [SerializeField] float aimSpeed;
    [SerializeField] Vector3 offset;

    void Start()
    {
        if(!targetTransform)
            targetTransform = GameObject.Find(targetTransformName).transform;
    }

    public float GetAngleFromTarget()
    {
        return Vector3.Angle(boneTransform.up, (targetTransform.position + offset) - boneTransform.position);
    }

    void Update()
    {
        if(!rootTransform || !boneTransform)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dirToPlayer = targetTransform.position - (boneTransform.position - offset);
        
        Vector3 limitedDir = Vector3.RotateTowards(
            rootTransform.rotation * rootTransform.forward, 
            dirToPlayer.normalized, 
            maxAngle * Mathf.Deg2Rad,
            0
        );

        transform.position = Vector3.Lerp(transform.position, boneTransform.position + limitedDir * dirToPlayer.magnitude, Time.deltaTime * aimSpeed);
    }
}