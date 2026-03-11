using UnityEngine;

public class TurretRotateTowardsPlayer : MonoBehaviour
{
    [SerializeField] Transform playerPosition;
    [SerializeField] float yawSpeed, pitchSpeed;
    [SerializeField] float range;

    private Quaternion baseYaw, basePitch;
    private Transform yawBone, pitchBone;

    void Start()
    {
        yawBone = transform.Find("Root/Yaw");
        pitchBone = transform.Find("Root/Yaw/Pitch");

        baseYaw = yawBone.localRotation;
        basePitch = pitchBone.localRotation;

        if(!playerPosition)
        {
            playerPosition = GameObject.FindWithTag("Player").transform;
        }
    }

    public float advanceTowardsPlayer()
    {
        float distance = (playerPosition.position - transform.position).magnitude;

        if(distance <= range)
        {
            Vector3 yawDirection = playerPosition.position - yawBone.position;

            Quaternion yawRotation = Quaternion.LookRotation(Quaternion.Inverse(transform.rotation) * yawDirection);

            Quaternion nextYaw = Quaternion.RotateTowards(
                yawBone.localRotation, 
                yawRotation, 
                yawSpeed * Time.deltaTime
            );

            yawBone.localRotation = Quaternion.Euler(
                yawBone.localRotation.eulerAngles.x, 
                nextYaw.eulerAngles.y, 
                yawBone.localRotation.eulerAngles.z
            );


            Vector3 pitchDirection = playerPosition.position - pitchBone.position;

            Quaternion pitchRotation = Quaternion.LookRotation(Quaternion.Inverse(transform.rotation) * pitchDirection);

            pitchRotation = pitchRotation * basePitch;

            Quaternion nextPitch = Quaternion.RotateTowards(
                pitchBone.localRotation, 
                pitchRotation, 
                pitchSpeed * Time.deltaTime
            );

            pitchBone.localRotation = Quaternion.Euler(
                nextPitch.eulerAngles.x, 
                pitchBone.localRotation.eulerAngles.y,
                pitchBone.localRotation.eulerAngles.z
            );

            return Vector3.Angle(pitchDirection, pitchBone.up);
        }
        else
        {
            Quaternion nextYaw = Quaternion.RotateTowards(
                yawBone.localRotation, 
                baseYaw, 
                yawSpeed * Time.deltaTime
            );

            yawBone.localRotation = Quaternion.Euler(
                yawBone.localRotation.eulerAngles.x, 
                nextYaw.eulerAngles.y, 
                yawBone.localRotation.eulerAngles.z
            );

            Quaternion nextPitch = Quaternion.RotateTowards(
                pitchBone.localRotation, 
                basePitch, 
                pitchSpeed * Time.deltaTime
            );

            pitchBone.localRotation = Quaternion.Euler(
                nextPitch.eulerAngles.x, 
                pitchBone.localRotation.eulerAngles.y,
                pitchBone.localRotation.eulerAngles.z
            );

            return 180;
        }
    }
}
