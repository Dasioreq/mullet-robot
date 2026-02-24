using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GeneratorBehaviour : MonoBehaviour
{
    [SerializeField] private List<Room> rooms;
    [SerializeField] private Room origin;
    [SerializeField] private LayerMask roomMask;

    Vector3 position;
    Vector3 direction;

    GameObject originInstance;

    List<int> indices = new List<int>();

    List<(GameObject instance, Vector3 position, Vector3 direction, int index, List<int> possibleIndices)> generatedRooms;

    void Init()
    {
        position = new Vector3(0, 0, -7.5f);
        direction = origin.exitDirection;

        originInstance = origin.CreateInstance(position, direction);

        position += origin.exit;

        for(int i = 0; i < rooms.Count; i++)
        {
            for(int j = 0; j < rooms[i].weight; j++)
            {
                indices.Add(i);
            }
        }

        generatedRooms = new List<(GameObject, Vector3, Vector3, int, List<int>)>{(originInstance, Vector3.zero, origin.exitDirection, -1, new List<int>(indices))};
    }

    IEnumerator Generate(uint roomNumber)
    {
        for(int roomIndex = 1; roomIndex <= roomNumber; roomIndex++)
        {
            var lastRoom = generatedRooms[roomIndex - 1];

            var availibleIndices = lastRoom.possibleIndices;

            if(availibleIndices.Count == 0)
            {
                if(roomIndex >= 1)
                {
                    Destroy(lastRoom.instance);
                    position = lastRoom.position;
                    direction = lastRoom.direction;
                    generatedRooms[roomIndex - 2].possibleIndices.RemoveAll(el => el == lastRoom.index);
                    generatedRooms.RemoveAt(roomIndex - 1);
                    roomIndex -= 2;
                    yield return null;
                    continue;
                }
            }

            int i = availibleIndices[UnityEngine.Random.Range(0, availibleIndices.Count)];
            
            Room r = rooms[i];

            GameObject instance = r.CreateInstance(position, direction);

            if(Room.isColliding(instance, generatedRooms.Select(room => room.instance).ToArray<GameObject>()[..^1]))
            {
                Destroy(instance);

                availibleIndices.RemoveAll(el => el == i);
                roomIndex--;
            }
            else
            {
                generatedRooms.Add((instance, position, direction, i, new List<int>(indices)));

                float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
                position += rotation * r.exit;
                direction = rotation * r.exitDirection;
            }

            yield return null;
        }
    }

    void Awake()
    {
        Init();
        StartCoroutine(Generate(50));
    }
}
