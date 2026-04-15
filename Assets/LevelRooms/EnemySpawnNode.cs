using UnityEngine;

/// @brief A template for an enemy spawn node
/// 
/// Defines the enemies that may spawn in that position, as well as how likely they are to spawn.
public class EnemySpawnNode : MonoBehaviour
{
    [SerializeField] GameObject[] possibleEnemies;
    [SerializeField] public float chance;

    /// @brief Instantiates a random enemy prefab at its own Transform
    public GameObject Spawn()
    {
        int index = Random.Range(0, possibleEnemies.Length);

        return Instantiate(possibleEnemies[index], transform.position, transform.rotation);
    }
}
