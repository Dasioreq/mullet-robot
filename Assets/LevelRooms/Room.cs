using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/Room")]
public class Room : ScriptableObject
{
    [SerializeField] public Vector3 exit;
    [SerializeField] public Vector3 exitDirection;
    [SerializeField] public GameObject prefab;
    [SerializeField] public uint weight = 1;

    public GameObject CreateInstance(Vector3 position, Vector3 direction)
    {
        return Instantiate(prefab, position, Quaternion.LookRotation(direction));
    }

    public static bool isColliding(GameObject instantiated, GameObject[] existingRooms)
    {
        var boundingBox = instantiated.transform.Find("Bounds").GetComponent<Collider>().bounds;

        // Vector3 center = boundingBox.center;
        // Vector3 extents = boundingBox.extents - Vector3.one * 0.05f;

        // Quaternion rotation = Quaternion.LookRotation(direction);

        // // float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        // // center = Quaternion.AngleAxis(angle, Vector3.up) * center;
        // // extents = Quaternion.AngleAxis(angle, Vector3.up) * extents;

        // // center += position;
        // // Vector3 newDirection = Quaternion.AngleAxis(angle, Vector3.up) * exitDirection;
        // // center += (newDirection + direction).normalized;

        // Debug.DrawLine(center - extents, center + extents, Color.red, 99999);

        // Physics.SyncTransforms();

        // Collider[] collisions = Physics.OverlapBox(
        //     center,
        //     extents,
        //     Quaternion.identity,
        //     layer
        // );

        foreach(GameObject room in existingRooms)
        {
            Collider collision = room.transform.Find("Bounds").GetComponent<Collider>();

            if((instantiated.transform.position - room.transform.position).magnitude > boundingBox.size.magnitude + collision.bounds.size.magnitude)
                continue;
            
            if(boundingBox.Intersects(collision.bounds))
                return true;
        }

        return false;
    }
}
