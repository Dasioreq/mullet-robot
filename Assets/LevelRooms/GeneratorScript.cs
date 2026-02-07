using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneratorBehavour : MonoBehaviour
{
    [SerializeField] private List<Room> rooms;
    [SerializeField] private Room origin;

    void generate(uint roomNumber)
    {
        Vector3 position = Vector3.zero;
        Vector3 direction = origin.exitDirection;

        origin.CreateInstance(position, direction);

        position += origin.exit;
        direction = origin.exitDirection;

        for(uint _ = 0; _ < roomNumber; _++)
        {
            int i = UnityEngine.Random.Range(0, rooms.Count);

            Room r = rooms[i];
            r.CreateInstance(position, direction);

            position += Quaternion.AngleAxis(Vector3.Angle(direction, Vector3.forward), Vector3.up) * r.exit;

            float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            direction = Quaternion.AngleAxis(angle, Vector3.up) * r.exitDirection;
        }
    }

    void Awake()
    {
        generate(8);
    }
}
