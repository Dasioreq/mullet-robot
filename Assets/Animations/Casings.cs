using UnityEngine;

public class Casings : MonoBehaviour
{
    [SerializeField] GameObject casingPrefab;
    [SerializeField] Vector3 casingPosition;
    [SerializeField] Vector3 forceVector;
    [SerializeField] Vector3 deviationAngles;

    public void SpawnCasing(Transform parent, Quaternion rotation)
    {
        var casing = Instantiate(casingPrefab, parent.position + rotation * casingPosition, rotation, parent);

        var rb = casing.AddComponent<Rigidbody>();

        Quaternion deviation = Quaternion.Euler(Random.Range(-deviationAngles.x, deviationAngles.x), Random.Range(-deviationAngles.y, deviationAngles.y), Random.Range(-deviationAngles.z, deviationAngles.z));

        rb.linearVelocity = deviation * rotation * forceVector;
        rb.linearDamping = 0;
    }
}
