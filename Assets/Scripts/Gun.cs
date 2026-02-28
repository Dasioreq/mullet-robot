using System;
using UnityEngine;

    public class Gun : MonoBehaviour
{
    [SerializeField] private float gunDamage;
    [SerializeField] public Camera cam;
    [SerializeField] private float fireCooldown;
    [SerializeField] private float reloadCooldown;
    [SerializeField] private uint projectileCount = 1;
    [SerializeField] private float spreadDeg;

    [SerializeField] public GameObject hitPrefab;

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

            if (Input.GetMouseButtonDown(0) && (ammo > 0 || maxAmmo == 0))
            {
                Fire();
            }
            else if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo)
            {
                Reload();
            }
            else if (Input.GetMouseButtonDown(0) && ammo == 0 && maxAmmo != 0)
            {
                EmptyReload();
            }
        }
    }

    void Fire()
    {
        RaycastHit hit;
        Vector3 origin = cam.transform.position;

        for(int i = 0; i < projectileCount; i++)
        {
            Vector3 direction = cam.transform.forward;

            Quaternion spreadYaw = Quaternion.AngleAxis(UnityEngine.Random.Range(0, spreadDeg), cam.transform.up);
            Quaternion spreadRoll = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), cam.transform.forward);

            direction = spreadRoll * (spreadYaw * direction);

            if(Physics.Raycast(origin, direction, out hit))
            {
                var obj = hit.transform.gameObject;
                if(obj)
                {
                    var damageHandler = obj.GetComponent<DamageHandler>();
                    if(damageHandler)
                        damageHandler.GetDamaged(gunDamage);
                }
            }
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
    void EmptyReload()
    {
        reloading = true;

        var gunActions = GetComponentsInChildren<Actions>();

        foreach (Actions action in gunActions)
        {
            StartCoroutine(action.EmptyReload());
        }

        cooldown = reloadCooldown;
    }
}
