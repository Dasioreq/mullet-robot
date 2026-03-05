using UnityEngine;
using System.Collections;
using static Unity.VisualScripting.Member;

public class TurretDamage : EnemyDamage
{
    [SerializeField] GameObject[] explosionParticles;
    [SerializeField] AudioClip deadSound;
    protected AudioSource source;
    public void Start()
    {
        source = GetComponent<AudioSource>();
    }

    override public void Destroy()
    {
        base.Destroy();
        var yawBone = transform.Find("Root/Yaw").gameObject;
        StartCoroutine(Explode(yawBone.transform.position));
        Destroy(yawBone);
        Destroy(GetComponent<TurretAI>());
    }

    public IEnumerator Explode(Vector3 position)
    {
        foreach (var obj in explosionParticles)
        {
            foreach(var p in explosionParticles)
            {
                foreach(ParticleSystem particle in p.GetComponentsInChildren<ParticleSystem>())
                {
                    Instantiate(particle, position, Quaternion.LookRotation(transform.up));
                }
            }
        }
        yield return new WaitForSeconds(0.05f);
        source.PlayOneShot(deadSound,6);
        yield break;
    }
}
