using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField] List<GameObject> gunPrefabs;
    [SerializeField] GameObject gunHolder;
    [SerializeField] Camera cam;
    [SerializeField] Cards cardPanelScript;
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
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Equip(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Equip(2);
        }
    }
}
