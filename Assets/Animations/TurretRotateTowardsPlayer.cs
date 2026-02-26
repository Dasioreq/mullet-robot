using UnityEngine;

public class TurretRotateTowardsPlayer : MonoBehaviour
{
    [SerializeField] Transform playerPosition;
    [SerializeField] float degreesPerSecond;
    [SerializeField] float range;

    private Quaternion baseYaw, basePitch;
    private Transform yawBone, pitchBone;

    void Start()
    {
        yawBone = transform.Find("Root/Yaw");
        pitchBone = transform.Find("Root/Yaw/Pitch");

        baseYaw = yawBone.rotation;
        basePitch = pitchBone.rotation;
    }

    void Update()
    {
        Vector3 direction = playerPosition.position - transform.position;

        if(direction.magnitude <= range)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion nextYaw = Quaternion.RotateTowards(
                yawBone.rotation, 
                targetRotation, 
                degreesPerSecond * Time.deltaTime
            );

            yawBone.rotation = Quaternion.Euler(
                yawBone.rotation.eulerAngles.x, 
                nextYaw.eulerAngles.y, 
                yawBone.rotation.eulerAngles.z
            );

            targetRotation = targetRotation * Quaternion.Euler(-transform.rotation.eulerAngles);

            Quaternion nextPitch = Quaternion.RotateTowards(
                pitchBone.rotation, 
                targetRotation, 
                degreesPerSecond * Time.deltaTime
            );

            pitchBone.rotation = Quaternion.Euler(
                nextPitch.eulerAngles.x, 
                yawBone.rotation.eulerAngles.y,
                yawBone.rotation.eulerAngles.z
            );
        }
        else
        {
            Quaternion nextYaw = Quaternion.RotateTowards(
                yawBone.rotation, 
                baseYaw, 
                degreesPerSecond * Time.deltaTime
            );

            yawBone.rotation = Quaternion.Euler(
                yawBone.rotation.eulerAngles.x, 
                nextYaw.eulerAngles.y, 
                yawBone.rotation.eulerAngles.z
            );

            Quaternion nextPitch = Quaternion.RotateTowards(
                pitchBone.rotation, 
                basePitch, 
                degreesPerSecond * Time.deltaTime
            );

            pitchBone.rotation = Quaternion.Euler(
                nextPitch.eulerAngles.x, 
                yawBone.rotation.eulerAngles.y,
                yawBone.rotation.eulerAngles.z
            );
        }
    }
}
