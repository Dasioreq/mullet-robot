using System.Collections.Generic;
using UnityEngine;

/// @class EquipWeapon
/// @brief Script handilng changing the player's weapon
public class EquipWeapon : MonoBehaviour
{
    [SerializeField] List<GameObject> gunPrefabs;
    [SerializeField] public GameObject gunHolder;
    [SerializeField] Camera cam;
    [SerializeField] Cards cardPanelScript;
    [SerializeField] GameObject crosshairCanvas;
    public int currentWeapon = -1;
    GameObject crosshair = null;

    /// @brief Instantiates the weapon prefab based on a given ID, fills in missing Component references and changes the UI Crosshair to the weapons's
    /// @param index The weapon ID
    public void Equip(int index)
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
        else if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            Equip(7);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            Equip(8);
        }
    }
}
