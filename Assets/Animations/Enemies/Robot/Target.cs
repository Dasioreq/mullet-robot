using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem.XR;

public class Target : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] string targetTransformName;
    [SerializeField] float range;
    [SerializeField] Transform rootTransform;
    [SerializeField] Vector3 rootForward;
    [SerializeField] Transform boneTransform;
    [SerializeField] float maxAngle;
    [SerializeField] float aimSpeed;
    [SerializeField] Vector3 offset;

    [Serializable]
    public struct AxisLock
    {
        public bool x, y, z;
    }

    [SerializeField] AxisLock lockedAxes;


    void Start()
    {
        if(rootForward == Vector3.zero)
            rootForward = Vector3.up;
        if(!targetTransform)
            targetTransform = GameObject.Find(targetTransformName).transform;
    }

    public float GetAngleFromTarget()
    {
        if(targetTransform)
        {
            Vector3 horizontalTargetPos = new Vector3(targetTransform.position.x - boneTransform.position.x, 0, targetTransform.position.z - boneTransform.position.z).normalized;
            Vector3 horizontalUp = new Vector3(boneTransform.up.x, 0, boneTransform.up.z).normalized;

            return Vector3.Angle(horizontalUp, horizontalTargetPos);            
        }
        else
            return 180;
    }

    void Update()
    {
        if(!rootTransform || !boneTransform)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dirToPlayer = targetTransform.position - (boneTransform.position - offset);

        dirToPlayer = rootTransform.InverseTransformDirection(dirToPlayer);

        dirToPlayer = Vector3.Scale(dirToPlayer, new Vector3(lockedAxes.x ? 0 : 1, lockedAxes.y ? 0 : 1, lockedAxes.z ? 0 : 1));

        dirToPlayer = rootTransform.TransformDirection(dirToPlayer);
        
        Vector3 limitedDir = Vector3.RotateTowards(
            rootTransform.rotation * rootForward, 
            dirToPlayer.normalized, 
            maxAngle * Mathf.Deg2Rad,
            0
        );

        transform.position = Vector3.Lerp(transform.position, boneTransform.position + limitedDir * dirToPlayer.magnitude, Time.deltaTime * aimSpeed);

        Vector3 lockablePos = transform.position;

        if(lockedAxes.x)
            lockablePos.x = boneTransform.forward.x;
        if(lockedAxes.y)
            lockablePos.y = boneTransform.forward.y;
        if(lockedAxes.z)
            lockablePos.z = boneTransform.forward.z;

        transform.position = lockablePos;
    }
}