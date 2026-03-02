using UnityEngine;

public class EnemyDamage : DamageHandler
{
    public float health;

    public override void GetDamaged(float damage)
    {
        health -= damage;
        if(health <= 0)
            Destroy(gameObject);
    }
}
