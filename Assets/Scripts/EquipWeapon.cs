using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField] List<GameObject> gunPrefabs;
    [SerializeField] GameObject gunHolder;
    [SerializeField] Camera cam;
    [SerializeField] Cards cardPanelScript;
    [SerializeField] GameObject crosshairCanvas;
    int currentWeapon = -1;
    GameObject crosshair = null;

    void Equip(int index)
    {
        if(index >= gunPrefabs.Count || index == currentWeapon)
            return;

        currentWeapon = index;
        
        Destroy(gunHolder.GetComponentInChildren<Gun>().gameObject);
        
        var equippedGun = Instantiate(gunPrefabs[index], gunHolder.transform);
        equippedGun.GetComponent<ViewmodelSway>().player = gameObject;
        equippedGun.GetComponent<Gun>().cam = cam;

        if(crosshair)
            Destroy(crosshair);
        crosshair = Instantiate(equippedGun.GetComponent<Gun>().crosshairSprite, crosshairCanvas.transform);

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
        else if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            Equip(6);
        }
    }
}
