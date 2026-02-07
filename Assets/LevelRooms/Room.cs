using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/Room")]
public class Room : ScriptableObject
{
    [SerializeField] public Vector3 exit;
    [SerializeField] public Vector3 exitDirection;
    [SerializeField] public GameObject prefab;

    public GameObject CreateInstance(Vector3 position, Vector3 direction)
    {
        return Instantiate(prefab, position, Quaternion.LookRotation(direction));
    }
}
