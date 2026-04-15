using UnityEngine;

/// @class EnemyProjectile
/// @brief Inherits from \ref ProjectylesAmmo; Used by enemies
public class EnemyProjectile : ProjectylesAmmo
{
    /// @brief Implements dealing damage to the player and spawning hit praticles on impact with a collider implementing \ref IHitImpact
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

        Destroy(gameObject);
    }
}
