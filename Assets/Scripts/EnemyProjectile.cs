using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ProjectylesAmmo
{
    protected override void OnHit(RaycastHit hit)
    {
        PlayerDamage dmg;
        if(dmg = hit.collider.gameObject.GetComponentInParent<PlayerDamage>())
        {
            if(dmg.GetLifeTime() > 0)
                dmg.DamageWithEffect(damage);
        }

        HitParticles hp;
        if(hp = hit.collider.gameObject.GetComponentInParent<HitParticles>())
            hp.OnHit(hit, damage * 5);

        Destroy(this);
    }
}
