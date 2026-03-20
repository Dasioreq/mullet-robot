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
    [SerializeField] public GameObject crosshairSprite;
    [SerializeField] bool automatic = false;

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

            if(!automatic)
            {
                if (Input.GetMouseButtonDown(0) && (ammo > 0 || maxAmmo == 0))
                {
                    Fire();
                }
                else if (Input.GetMouseButtonDown(0) && ammo == 0 && maxAmmo != 0)
                {
                    EmptyReload();
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && (ammo > 0 || maxAmmo == 0))
                {
                    Fire();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo)
            {
                Reload();
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

            if(Physics.Raycast(origin, direction, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Bounds"))))
            {
                var obj = hit.transform.gameObject;
                if(obj)
                {
                    IHittable hittable = obj.GetComponentInParent<IHittable>();
                    if(hittable != null)
                    {
                        hittable.OnHit(hit, gunDamage);
                    }
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
