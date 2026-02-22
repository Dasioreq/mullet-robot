using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField] List<GameObject> gunPrefabs;
    [SerializeField] GameObject gunHolder;
    [SerializeField] Camera cam;
    [SerializeField] Cards cardPanelScript;
    [SerializeField] GameObject hitPrefab;
    int currentWeapon = 0;

    void Equip(int index)
    {
        if(index >= gunPrefabs.Count || index == currentWeapon)
            return;

        currentWeapon = index;
        
        Destroy(gunHolder.GetComponentInChildren<Gun>().gameObject);
        
        var equippedGun = Instantiate(gunPrefabs[index], gunHolder.transform);
        equippedGun.GetComponent<ViewmodelSway>().player = gameObject;
        equippedGun.GetComponent<Gun>().cam = cam;
        equippedGun.GetComponent<Gun>().hitPrefab = hitPrefab;

        cardPanelScript.weapon = equippedGun;
    }

    void Start()
    {
        Equip(0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Equip(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Equip(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Equip(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            Equip(3);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            Equip(4);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            Equip(5);
        }
    }
}
