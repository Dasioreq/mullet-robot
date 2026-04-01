using UnityEngine;

public class EnemySpawnNode : MonoBehaviour
{
    [SerializeField] GameObject[] possibleEnemies;
    [SerializeField] public float chance;

    public GameObject Spawn()
    {
        int index = Random.Range(0, possibleEnemies.Length - 1);

        return Instantiate(possibleEnemies[index], transform.position, transform.rotation);
    }
}
