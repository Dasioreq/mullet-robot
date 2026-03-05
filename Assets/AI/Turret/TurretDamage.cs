using UnityEngine;

public class TurretDamage : EnemyDamage
{
    [SerializeField] GameObject[] explosionParticles;

    override public void Destroy()
    {
        base.Destroy();
        var yawBone = transform.Find("Root/Yaw").gameObject;
        Explode(yawBone.transform.position);
        Destroy(yawBone);
        Destroy(GetComponent<TurretAI>());
    }

    void Explode(Vector3 position)
    {
        foreach(var obj in explosionParticles)
        {
            foreach(var p in explosionParticles)
            {
                foreach(ParticleSystem particle in p.GetComponentsInChildren<ParticleSystem>())
                {
                    Instantiate(particle, position, Quaternion.LookRotation(transform.up));
                }
            }
        }
    }
}
