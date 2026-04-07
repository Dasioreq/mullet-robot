using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] AudioMixerGroup Group;
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
                PlayClipAtPoint(ricochets[chance2], hit.point,1,Group);
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
    public static void PlayClipAtPoint
(AudioClip clip, Vector3 position, float volume = 1.0f, AudioMixerGroup group = null)
    {
        if (clip == null) return;
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        if (group != null)
            audioSource.outputAudioMixerGroup = group;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy(gameObject, clip.length *
(Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
    }
}
