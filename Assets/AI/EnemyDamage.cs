using UnityEngine;
using static GameController;

/// @interface IDamagable
/// @brief Implements \ref IHittable with damage and destruction handling
public interface IDamagable : IHittable
{
    /// @brief Handles what happens when damaged
    public void Damage(float damage);
    /// @brief Handles what happens when damaged enough to die
    public void Destroy();
}

/// @class EnemyDamage
/// @brief Implements \ref HitParticles and \ref IDamagable; used for enemies
public class EnemyDamage : HitParticles, IDamagable
{
    [SerializeField] float maxHealth;
    [SerializeField] float restoredLifetime;
    float health;
    protected bool destroyed = false;
    
    virtual protected void Start()
    {
        health = maxHealth;
    }

    override public void OnHit(RaycastHit hit, float damage)
    {
        base.OnHit(hit, damage);
        if(!destroyed)
            Damage(damage);
    }

    virtual public void Damage(float damage)
    {
        health -= damage;
        if(health <= 0)
            Destroy();
    }

    virtual public void Destroy()
    {
        destroyed = true;
        gameController.OnPlayerKillEnemy(restoredLifetime);
    }
}
