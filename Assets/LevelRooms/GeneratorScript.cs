using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneratorBehaviour : MonoBehaviour
{
    [SerializeField] private List<Room> rooms;
    [SerializeField] private Room origin;
    [SerializeField] private LayerMask roomMask;

    void generate(uint roomNumber)
    {
        Vector3 position = Vector3.zero;
        Vector3 direction = origin.exitDirection;

        position += origin.exit;

        List<int> indices = new List<int>();
        for(int i = 0; i < rooms.Count; i++)
        {
            for(int j = 0; j < rooms[i].weight; j++)
            {
                indices.Add(i);
            }
        }

        List<(GameObject instance, Vector3 position, Vector3 direction, int index, List<int> possibleIndices)> generatedRooms = new List<(GameObject, Vector3, Vector3, int, List<int>)>{(origin.CreateInstance(position, direction), Vector3.zero, origin.exitDirection, -1, new List<int>(indices))};

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
        }
    }

    void Awake()
    {
        generate(100);
    }
}
