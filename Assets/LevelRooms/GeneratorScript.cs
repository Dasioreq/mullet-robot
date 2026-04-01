using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GeneratorBehaviour : MonoBehaviour
{
    [SerializeField] private List<Room> rooms;
    [SerializeField] private Room origin;
    [SerializeField] private LayerMask roomMask;

    [Header("Light culling")]
    [SerializeField] float LightCullingDistance;
    [SerializeField] LayerMask lightLayer;
    private CullingGroup cullingGroup;
    List<Light> levelLights = new List<Light>();
    BoundingSphere[] boundingSpheres;

    Vector3 position;
    Vector3 direction;

    GameObject originInstance;

    List<int> indices = new List<int>();

    List<(GameObject instance, Vector3 position, Vector3 direction, int index, List<int> possibleIndices)> generatedRooms = new List<(GameObject instance, Vector3 position, Vector3 direction, int index, List<int> possibleIndices)>();
    List<GameObject> enemies = new List<GameObject>();

    public IEnumerator Generate(int roomNumber)
    {
        if(cullingGroup != null)
        {
            cullingGroup.Dispose();
            cullingGroup = null;
        }
        
        levelLights = new List<Light>();

        foreach(var room in generatedRooms)
        {
            if(room.instance)
                Destroy(room.instance);
        }

        foreach(var enemy in enemies)
        {
            if(enemy)
                Destroy(enemy);
        }

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
        enemies.Clear();

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

            Room r;
            int i;

            if(roomIndex == roomNumber)
            {
                r = origin;
                i = -1;
            }
            else
            {
                i = availibleIndices[UnityEngine.Random.Range(0, availibleIndices.Count)];
                r = rooms[i];
            }

            GameObject instance = r.CreateInstance(position, direction);

            if(Room.isColliding(instance, generatedRooms.Select(room => room.instance).ToArray<GameObject>()[..^1]))
            {
                Destroy(instance);

                availibleIndices.RemoveAll(el => el == i || roomIndex == roomNumber);
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

        generatedRooms.Last().instance.GetComponent<IntermisionActions>().levelGenerator = this;
        generatedRooms.Last().instance.GetComponent<IntermisionActions>().endLevel = true;

        foreach(var room in generatedRooms)
        {
            enemies.AddRange(Room.SpawnEnemies(room.instance));

            foreach(var light in room.instance.GetComponentsInChildren<Light>())
            {
                if((lightLayer.value & (1 << light.gameObject.layer)) != 0)
                {
                    levelLights.Add(light);
                    light.enabled = false;
                }
            }
        }

        InitializeCullingGroup();
    }

    void InitializeCullingGroup()
    {
        Light[] lights = levelLights.ToArray();
        boundingSpheres = new BoundingSphere[lights.Length];

        foreach(var (light, i) in lights.Select((value, i) => (value, i)))
        {
            boundingSpheres[i] = new BoundingSphere(light.transform.position, light.range);
        }

        cullingGroup = new CullingGroup();
        cullingGroup.targetCamera = Camera.main;
        cullingGroup.SetBoundingSpheres(boundingSpheres);
        cullingGroup.SetBoundingSphereCount(lights.Length);
        cullingGroup.SetBoundingDistances(new float[] {LightCullingDistance});
        cullingGroup.SetDistanceReferencePoint(Camera.main.transform);

        cullingGroup.onStateChanged = OnStateChanged;

        foreach(var (light, i) in lights.Select((value, i) => (value, i)))
        {
            UpdateLightState(i);
        }
    }

    void OnStateChanged(CullingGroupEvent ev)
    {
        UpdateLightState(ev.index);
    }

    void UpdateLightState(int index)
    {
        bool inFrustum = cullingGroup.IsVisible(index);
        bool inDistance = cullingGroup.GetDistance(index) == 0; 

        levelLights[index].enabled = inFrustum && inDistance;
    }

    private void OnDestroy()
    {
        if(cullingGroup != null)
        {
            cullingGroup.Dispose();
            cullingGroup = null;
        }
    }

    void Awake()
    {
        // StartCoroutine(Generate(10));
    }

    public void Regenerate()
    {
        
    }
}
