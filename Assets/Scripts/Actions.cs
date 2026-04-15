using System.Collections;
using UnityEngine;

/**
*   @class Actions
*   @brief Base class for all visual and auditory weapon effects (eg. animations, sfx, muzzle flashes etc.)
*
*   This class is the base that all weapon anim-scripts inherit from.
*   It features base methods for firing, reloading and casting muzzle flashes.
*/
[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class Actions : MonoBehaviour
{
    [SerializeField] protected AudioClip shotSound;
    [SerializeField] protected AudioClip reloadSound;
    [SerializeField] protected float fireSfxDelay;
    [SerializeField] protected float reloadSfxDelay;

    protected Animator anim;
    protected AudioSource source;
    [SerializeField] protected ParticleSystem[] muzzleFlashes;
    protected Light[] muzzleFlashLights;

    /// @brief Initializes values and refences to Components
    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        muzzleFlashLights = GetComponentsInChildren<Light>();
    }

    /// @brief Plays the firing animation, sfx and muzzle flash
    virtual public IEnumerator Fire() { yield break; }
    /// @brief Plays the reloading animation and sfx
    virtual public IEnumerator Reload() { yield break; }
    /// @brief Plays the animation and sfx for firing with an empty mag
    virtual public IEnumerator EmptyReload() { yield break; }

    /// @brief Plays the muzzle flash effects
    public IEnumerator Flash(float time)
    {
        foreach (var flash in muzzleFlashes)
        {
            flash.Play();
        }
        foreach (var light in muzzleFlashLights)
        {
            light.enabled = true;
        }
        yield return new WaitForSeconds(time);
        foreach (var light in muzzleFlashLights)
        {
            light.enabled = false;
        }
    }
}
