using UnityEngine;
using UnityEngine.Audio;

/// @interface IHittable
/// @brief Defines what happens when an implementing class gets hit with a hitscan
public interface IHittable
{
    /// @brief Defines what happens when hit with a certain amount of damage
    public virtual void OnHit(RaycastHit hit, float damage){}
}

/// @interface IHitImpact
/// @brief Inherits from \ref IHittable; Adds hit particles on impact
public interface IHitImpact: IHittable
{
    /// #brief Spawns particles depending on the hit position/damage
    public void SpawnParticles(RaycastHit hit, float damage);
}

/// @class HitParticles
/// @brief Implements IHitImpact; Adds basic, reusable logic for the hit particles
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

    /// @brief A helper function similar to AudioSource.PlayClipAtPoint(), but with added volume control
    /// Instantiates a temporary GameObject whose sole purpose in life is to play the AudioClip and die. Poetic.
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
