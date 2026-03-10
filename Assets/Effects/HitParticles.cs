using UnityEngine;
using static Unity.VisualScripting.Member;

public interface IHittable
{
    public virtual void OnHit(RaycastHit hit, float damage){}
}

public interface IHitImpact: IHittable
{
    public void SpawnParticles(RaycastHit hit, float damage);
}

public class HitParticles : MonoBehaviour, IHitImpact
{
    [SerializeField] GameObject[] particles;
    [SerializeField] AudioClip [] ricochets;
    virtual public void OnHit(RaycastHit hit, float damage)
    {
        this.SpawnParticles(hit, damage);
    }

    virtual public void SpawnParticles(RaycastHit hit, float damage)
    {
        int chance = Random.Range(0, 2);
        if (chance < 1)
        {
            if (ricochets.Length > 0)
            {
                int chance2 = Random.Range(0, ricochets.Length);
                AudioSource.PlayClipAtPoint(ricochets[chance2], hit.point);
            }
        }
        foreach (var p in particles)
            {
                Vector3 position = hit.point;
                Vector3 normal = hit.normal;
                foreach (ParticleSystem particle in p.GetComponentsInChildren<ParticleSystem>())
                {
                    var instance = Instantiate(particle, position, Quaternion.LookRotation(normal));
                    var main = instance.main;
                    var emmision = instance.emission;

                    emmision.rateOverTime = 1 / main.duration * damage;
                }
            }
    }
}