using UnityEngine;

public class PlayerDamage : MonoBehaviour, IDamagable
{
    [SerializeField] float maxLifeTime;
    float lifeTime;

    void Start()
    {
        lifeTime = maxLifeTime;
    }

    void Update()
    {
        if(lifeTime <= 1.0f)
        {
            Damage(Time.deltaTime * 0.5f);
        }
        else if(lifeTime <= 2.0f)
        {
            Damage(Time.deltaTime * 0.75f);
        }
        else
        {
            Damage(Time.deltaTime);
        }
    }

    virtual public void OnHit(RaycastHit hit, float damage)
    {
        Damage(damage);
    }

    virtual public void Damage(float damage)
    {
        lifeTime -= damage;
        if(lifeTime <= 0)
            Destroy();
    }

    virtual public void Destroy() {lifeTime = maxLifeTime;}
}
