using System.Collections;
using UnityEngine;

/**
*   @brief Base class for all visual and auditory enemy effects (eg. animations, sfx etc.)
*
*   This class is the base that all enemy anim-scripts inherit from.
*   It features base methods for attackingm, moving and playing muzzle flash particle systems.
*/
[RequireComponent(typeof(AudioSource))]
public class EnemyActions : MonoBehaviour
{
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected AudioClip moveSound;
    [SerializeField] protected float attackSfxDelay;
    [SerializeField] protected float moveSfxDelay;

    protected Animator anim;
    protected AudioSource source;
    protected ParticleSystem[] muzzleFlashes;
    protected Light[] muzzleFlashLights;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        muzzleFlashes = GetComponentsInChildren<ParticleSystem>();
        muzzleFlashLights = GetComponentsInChildren<Light>();
    }

    /// @brief Defines what an attack looks/sounds like
    virtual public IEnumerator Attack(){yield break;}
    /// @brief Defines what movement looks/sounds like
    virtual public IEnumerator Move(){yield break;}

    /// @brief Plays the muzzle flash effects
    public IEnumerator Flash(float time)
    {
        foreach(var flash in muzzleFlashes)
        {
            if(flash)
                flash.Play();
        }
        foreach(var light in muzzleFlashLights)
        {
            if(light)
                light.enabled = true;
        }
        yield return new WaitForSeconds(time);
        foreach(var light in muzzleFlashLights)
        {
            if(light)
                light.enabled = false;
        }
    }
}
