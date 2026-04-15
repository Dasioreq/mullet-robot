using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// @class Room
/// @brief A template ScriptableObject for defining Rooms for the level generator
[CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/Room")]
public class Room : ScriptableObject
{
    [SerializeField] public Vector3 exit;
    [SerializeField] public Vector3 exitDirection;
    [SerializeField] public GameObject prefab;
    [SerializeField] public uint weight = 1;
    [SerializeField] int minEnemies;
    [SerializeField] int maxEnemies;

    /// @brief Instantiates the room prefab at a give position and direction
    public GameObject CreateInstance(Vector3 position, Vector3 direction)
    {
        var instance = Instantiate(prefab, position, Quaternion.LookRotation(direction));

        return instance;
    }

    /// @brief Performs an intersection check between one instance and an array of previously generated instances
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

    /// @brief Returns and array of enemy Instances to be spawned in the level
    /// The enemies are defiend in the room's prefab's hierarchy, being children of one "EnemySpawnNodes" with the \ref EnemySpawnNode Component
    public GameObject[] SpawnEnemies(GameObject instance)
    {
        var nodeParent = instance.transform.Find("EnemySpawnNodes");
        List<GameObject> enemies = new List<GameObject>();
        if(nodeParent)
        {
            var nodes = nodeParent.GetComponentsInChildren<EnemySpawnNode>().ToList();

            int max = Mathf.Min(maxEnemies, nodes.Count);

            do
            {
                var usedNodes = new List<EnemySpawnNode>();

                foreach(var node in nodes)
                {
                    if(Random.Range(0.0f, 1.0f) <= node.chance || node.chance == 1f)
                    {
                        enemies.Add(node.Spawn());
                        usedNodes.Add(node);
                        if(enemies.Count >= max)
                            return enemies.ToArray();
                    }
                }

                nodes.RemoveAll(el => usedNodes.Contains(el));
            }
            while(enemies.Count < minEnemies && nodes.Count > 0);
        }
        return enemies.ToArray();
    }
}
