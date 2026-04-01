using UnityEngine;

public interface IDamagable : IHittable
{
    public void Damage(float damage);
    public void Destroy();
}

public class EnemyDamage : HitParticles, IDamagable
{
    [SerializeField] float maxHealth;
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
    }
}
