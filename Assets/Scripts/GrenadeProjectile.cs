using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// @class GrenadeProjectile
/// @brief Inherits from \ref ProjectylesAmmo; used by the Grenade Launcher
public class GrenadeProjectile : ProjectylesAmmo
{
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] AudioClip explotion;
    [SerializeField] AudioMixerGroup Group;
    
    /// @brief Implements dealing damage to each enemy in a given radius
    protected override void OnHit(RaycastHit hit)
    {
        Instantiate(explosionParticle, hit.point, Quaternion.identity);
        HitParticles.PlayClipAtPoint(explotion,hit.point,500.0f,Group);
        HashSet<EnemyDamage> damageScripts = new HashSet<EnemyDamage>();
        foreach (var collider in Physics.OverlapSphere(hit.point, explosionRadius))
        {
            EnemyDamage dmg;

            if(dmg = collider.gameObject.GetComponentInParent<EnemyDamage>())
            {
                if(dmg.enabled)
                    damageScripts.Add(dmg);
            }
        }
        foreach (var script in damageScripts)
        {
            script.Damage(damage);
        }
    }
}