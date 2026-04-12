using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GrenadeProjectile : ProjectylesAmmo
{
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] AudioClip explotion;
    [SerializeField] AudioMixerGroup Group;
    protected override void OnHit(RaycastHit hit)
    {
        Instantiate(explosionParticle, hit.point, Quaternion.identity);
        HitParticles.PlayClipAtPoint(explotion,hit.point,500.0f,Group);
        foreach (var collider in Physics.OverlapSphere(hit.point, explosionRadius))
        {
            EnemyDamage dmg;

            HashSet<EnemyDamage> damageScripts = new HashSet<EnemyDamage>();
            if (dmg = collider.gameObject.GetComponentInParent<EnemyDamage>())
            {
                if (dmg.enabled)
                    damageScripts.Add(dmg);
            }

            foreach (var script in damageScripts)
            {
                script.Damage(damage);
                Debug.Log(script.gameObject);
            }
        }
    }
}