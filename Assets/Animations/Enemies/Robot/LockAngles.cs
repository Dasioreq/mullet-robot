using UnityEngine;

public class LockAngles : MonoBehaviour
{
    [SerializeField] bool lockX;
    [SerializeField] bool lockY;
    [SerializeField] bool lockZ;

    Vector3 baseEulers;

    void Start()
    {
        baseEulers = transform.localEulerAngles;
    }

    void LateUpdate()
    {
        Vector3 eulers = transform.localEulerAngles;

        if(lockX)
            eulers.x = baseEulers.x;
        if(lockY)
            eulers.y = baseEulers.y;
        if(lockZ)
            eulers.z = baseEulers.z;

        transform.localEulerAngles = eulers;
    }
}
