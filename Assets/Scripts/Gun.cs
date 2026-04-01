using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameController;

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

    List<EmitBulletParticle> bulletParticleEmitters = new List<EmitBulletParticle>();

    List<(Vector3 v, bool useDirection)> scheduledBulletParticles = new List<(Vector3 v, bool useDirection)>();

    void Start()
    {
        ammo = maxAmmo;

        foreach(var emitter in GetComponentsInChildren<ParticleSystem>())
        {
            if(emitter.gameObject.TryGetComponent<EmitBulletParticle>(out EmitBulletParticle bulletPart))
            {
                bulletParticleEmitters.Add(bulletPart);
            }
        }
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

            if(gameController.GetGameState() == GameState.Normal)
            {
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
    }

    void LateUpdate()
    {
        foreach(var scheduled in scheduledBulletParticles)
        {
            foreach(var emitter in bulletParticleEmitters)
            {
                if(scheduled.useDirection)
                    emitter.EmitDirection(scheduled.v);
                else
                    emitter.Emit(scheduled.v);
            }
        }

        scheduledBulletParticles.Clear();
    }

    void Fire()
    {
        RaycastHit hit;
        Vector3 origin = cam.transform.position;

        var gunActions = GetComponentsInChildren<Actions>();

        foreach(Actions action in gunActions)
        {
            StartCoroutine(action.Fire());
        }

        for(int i = 0; i < projectileCount; i++)
        {
            Vector3 direction = cam.transform.forward;

            Quaternion spreadYaw = Quaternion.AngleAxis(UnityEngine.Random.Range(0, spreadDeg), cam.transform.up);
            Quaternion spreadRoll = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), cam.transform.forward);

            direction = spreadRoll * (spreadYaw * direction);

            Vector3? hitPos = null;

            if(Physics.Raycast(origin, direction, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Bounds"))))
            {
                hitPos = hit.point;
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

            foreach(var emitter in bulletParticleEmitters)
            {
                if(hitPos != null)
                {
                    scheduledBulletParticles.Add((hitPos ?? Vector3.zero, false));
                }
                else
                {
                    scheduledBulletParticles.Add((direction, true));
                }
            }
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
