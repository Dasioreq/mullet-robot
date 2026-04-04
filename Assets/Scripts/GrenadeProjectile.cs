using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : ProjectylesAmmo
{
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionParticle;

    protected override void OnHit(RaycastHit hit)
    {
        Instantiate(explosionParticle, hit.point, Quaternion.identity);
        foreach(var collider in Physics.OverlapSphere(hit.point, explosionRadius))
        {
            EnemyDamage dmg;
            
            HashSet<EnemyDamage> damageScripts = new HashSet<EnemyDamage>();
            if(dmg = collider.gameObject.GetComponentInParent<EnemyDamage>())
            {
                if(dmg.enabled)
                    damageScripts.Add(dmg);
            }

            foreach(var script in damageScripts)
            {
                script.Damage(damage);
                Debug.Log(script.gameObject);
            }
        }
    }
}
