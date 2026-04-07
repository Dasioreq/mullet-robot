using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static GameController;

public class Gun : MonoBehaviour
{
    [SerializeField] protected float gunDamage;
    [SerializeField] public Camera cam;
    [SerializeField] protected float fireCooldown;
    [SerializeField] protected float reloadCooldown;
    [SerializeField] protected uint projectileCount = 1;
    [SerializeField] protected float spreadDeg;
    [SerializeField] public GameObject crosshairSprite;
    [SerializeField] bool automatic = false;
    [SerializeField] uint maxAmmo;
    protected uint ammo;
    bool reloading = false;
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private ParticleSystem[] chargeObjects;
    [SerializeField] private float[] thresholds;
    [SerializeField] bool canOverheat = false;
    [SerializeField] float overheatTime = 0.0f;
    float overheatTimer = 0.0f;
    float overheat = 0.0f;
    bool OnPlay = false;
    private bool[] onPlayStates;
    [SerializeField] float[] timeToFastest;
    [SerializeField] float[] fastestFireCooldown;
    private float fireCooldownNumber;

    protected float cooldown = .0f;

    List<EmitBulletParticle> bulletParticleEmitters = new List<EmitBulletParticle>();

    List<(Vector3 v, bool useDirection)> scheduledBulletParticles = new List<(Vector3 v, bool useDirection)>();

    void Start()
    {
        fireCooldownNumber = fireCooldown;
        ammo = maxAmmo;
        onPlayStates = new bool[chargeObjects.Length];

        foreach(var emitter in GetComponentsInChildren<ParticleSystem>())
        {
            if(emitter.gameObject.TryGetComponent<EmitBulletParticle>(out EmitBulletParticle bulletPart))
            {
                bulletParticleEmitters.Add(bulletPart);
            }
        }
    }

    protected void Update()
    {
        if (targetRenderer != null)
        {
            Color c = targetRenderer.material.GetColor("_BaseColor");
            c.a = overheat; 
            targetRenderer.material.SetColor("_BaseColor", c);
        }
        for (int i = 0; i < chargeObjects.Length; i++)
        {
            if (overheat >= thresholds[i])
            {
                if (!onPlayStates[i])
                {
                    chargeObjects[i].Play();
                    onPlayStates[i] = true;
                }
            }
            else
            {
                if (onPlayStates[i])
                {
                    chargeObjects[i].Stop();
                    onPlayStates[i] = false;
                }
            }
        }
        if (canOverheat)
        {
            if (overheat >= timeToFastest[1])
            {
                fireCooldown = fastestFireCooldown[1];
            }
            else if (overheat >= timeToFastest[0])
            {
                fireCooldown = fastestFireCooldown[0];
            }
            else
            {
                fireCooldown = fireCooldownNumber;
            }
        }
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            if (reloading)
            {
                ammo = maxAmmo;
                reloading = false;
            }

            if(gameController.GetGameState() == GameState.Normal)
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
                    else if (canOverheat)
                    {
                        overheatTimer = Mathf.Max(overheatTimer - Time.deltaTime, 0f);
                    }
                    if (overheatTime > 0)
                        overheat = overheatTimer / overheatTime;
                    
                }

            if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo)
            {
                Reload();
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

    protected virtual void Fire()
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
                if (obj)
                {
                    IHittable hittable = obj.GetComponentInParent<IHittable>();
                    if (hittable != null)
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

        if (canOverheat)
        {
            overheatTimer = Mathf.Min(overheatTimer + fireCooldown, overheatTime);
        }

        ammo--;

        cooldown = fireCooldown;
    }

    void Reload()
    {
        reloading = true;

        var gunActions = GetComponentsInChildren<Actions>();

        foreach (Actions action in gunActions)
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