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
        var instance = Instantiate(prefab, position, Quaternion.LookRotation(direction));

        return instance;
    }

    public static bool isColliding(GameObject instantiated, GameObject[] existingRooms)
    {
        var boundingBox = instantiated.transform.Find("Bounds").GetComponent<Collider>().bounds;

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

    public static void SpawnEnemies(GameObject instance)
    {
        var nodeParent = instance.transform.Find("EnemySpawnNodes");
        if(nodeParent)
        {
            var nodes = nodeParent.GetComponentsInChildren<EnemySpawnNode>();

            foreach(var node in nodes)
            {
                if(Random.Range(0.0f, 1.0f) <= node.chance)
                {
                    node.Spawn();
                }
            }
        }
    }
}
