using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/Room")]
public class Room : ScriptableObject
{
    [SerializeField] public Vector3 exit;
    [SerializeField] public Vector3 exitDirection;
    [SerializeField] public GameObject prefab;
    [SerializeField] public uint weight = 1;
    [SerializeField] int minEnemies;
    [SerializeField] int maxEnemies;

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

    public GameObject[] SpawnEnemies(GameObject instance)
    {
        var nodeParent = instance.transform.Find("EnemySpawnNodes");
        List<GameObject> enemies = new List<GameObject>();
        if(nodeParent)
        {
            var nodes = nodeParent.GetComponentsInChildren<EnemySpawnNode>();

            int max = Mathf.Min(maxEnemies, nodes.Length);

            do
            {
                foreach(var node in nodes)
                {
                    if(Random.Range(0.0f, 1.0f) <= node.chance || node.chance == 1f)
                    {
                        enemies.Add(node.Spawn());
                        if(enemies.Count >= max)
                            return enemies.ToArray();
                    }
                }
            }
            while(enemies.Count < minEnemies);
        }
        return enemies.ToArray();
    }
}
