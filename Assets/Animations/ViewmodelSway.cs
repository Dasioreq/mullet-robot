using UnityEngine;

public class ViewmodelSway : MonoBehaviour
{
    [SerializeField] public GameObject player;

    [Header("Move Sway")]
    [SerializeField] float speed;
    [SerializeField] float magnitude;

    [Header("Breathing")]
    [SerializeField] float breathingSpeed;
    [SerializeField] float breathingMagnitude;

    MovementHandler movementScript;
    Rigidbody playerRb;
    Vector3 basePosition;
    Vector3 targetPosition;

    void Start()
    {
        movementScript = player.GetComponent<MovementHandler>();
        playerRb = player.GetComponent<Rigidbody>();
        basePosition = transform.localPosition;
        targetPosition = basePosition;
    }

    Vector3 velocity = Vector3.zero;

    void Update()
    {
        if(movementScript.grounded)
        {
            float speedRatio = playerRb.linearVelocity.magnitude / movementScript.maxVelocity;
            float horizontalSway = Mathf.Sin(Time.time * speed) * magnitude * speedRatio;
            float verticalSway = Mathf.Cos(Time.time * speed * 2) * magnitude * speedRatio + Mathf.Sin(Time.time * breathingSpeed) * breathingMagnitude;

            targetPosition = basePosition + new Vector3(horizontalSway, verticalSway, 0);
        }
        else
        {
            targetPosition = basePosition;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime);
    }
}
