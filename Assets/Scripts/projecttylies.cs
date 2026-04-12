using UnityEngine;
using System;



public class Projecttylies : Gun
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    protected override void Fire()
    {
        Vector3 origin = cam.transform.position;

        for (int i = 0; i < projectileCount; i++)
        {
            Vector3 direction = cam.transform.forward;

            Quaternion spreadYaw = Quaternion.AngleAxis(UnityEngine.Random.Range(0, spreadDeg), cam.transform.up);
            Quaternion spreadRoll = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), cam.transform.forward);

            direction = spreadRoll * (spreadYaw * direction);

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
            proj.GetComponent<ProjectylesAmmo>().damage *= damageMultiplier;
        }

        var gunActions = GetComponentsInChildren<Actions>();
        foreach (Actions action in gunActions)
        {
            StartCoroutine(action.Fire());
        }

        ammo--;
        cooldown = fireCooldown;
    }
}
