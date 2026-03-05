using UnityEngine;

public interface IHittable
{
    public void OnHit(RaycastHit hit, float damage);
}

public interface IHitImpact: IHittable
{
    public void SpawnParticles(RaycastHit hit, float damage);
}

public class HitParticles : MonoBehaviour, IHitImpact
{
    [SerializeField] GameObject[] particles;

    virtual public void OnHit(RaycastHit hit, float damage)
    {
        this.SpawnParticles(hit, damage);
    }

    virtual public void SpawnParticles(RaycastHit hit, float damage)
    {
        foreach(var p in particles)
        {
            Vector3 position = hit.point;
            Vector3 normal = hit.normal;
            foreach(ParticleSystem particle in p.GetComponentsInChildren<ParticleSystem>())
            {
                var instance = Instantiate(particle, position, Quaternion.LookRotation(normal));
                var main = instance.main;
                var emmision = instance.emission;
                
                emmision.rateOverTime = 1 / main.duration * damage;
            }
        }
    }
}