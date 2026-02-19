using System;
using UnityEngine;

    public class Gun : MonoBehaviour
{
    [SerializeField] private float gunDamage;
    [SerializeField] public Camera cam;
    [SerializeField] private float fireCooldown;
    [SerializeField] private float reloadCooldown;

    [SerializeField] uint maxAmmo;
    uint ammo;
    bool reloading = false;

    private float cooldown = .0f;

    void Start()
    {
        ammo = maxAmmo;
    }

    private void Update()
    {
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            if(reloading)
            {
                ammo = maxAmmo;
                reloading = false;
            }

            if(Input.GetMouseButtonDown(0) && (ammo > 0 || maxAmmo == 0))
            {
                Fire();
            }
            else if(Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo)
            {
                Reload();
            }
        }
    }

    void Fire()
    {
        RaycastHit hit;
        Vector3 origin = cam.transform.position;
        Vector3 direction = cam.transform.forward;

        if (Physics.Raycast(origin, direction, out hit))
        {
            Debug.Log(hit.collider.gameObject.name + origin);
        }

        var gunActions = GetComponentsInChildren<Actions>();

        foreach(Actions action in gunActions)
        {
            StartCoroutine(action.Fire());
        }

        ammo--;

        cooldown = fireCooldown;
    }

    void Reload()
    {
        reloading = true;

        var gunActions = GetComponentsInChildren<Actions>();

        foreach(Actions action in gunActions)
        {
            StartCoroutine(action.Reload());
        }

        cooldown = reloadCooldown;
    }
}
