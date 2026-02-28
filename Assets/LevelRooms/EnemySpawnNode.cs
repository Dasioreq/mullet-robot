using UnityEngine;

public class EnemySpawnNode : MonoBehaviour
{
    [SerializeField] GameObject[] possibleEnemies;
    [SerializeField] public float chance;

    public void Spawn()
    {
        int index = Random.Range(0, possibleEnemies.Length - 1);

        Instantiate(possibleEnemies[index], transform.position, transform.rotation);
    }
}
