using System.Collections;
using UnityEngine;

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

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        muzzleFlashLights = GetComponentsInChildren<Light>();
    }

    virtual public IEnumerator Fire(){yield break;}
    virtual public IEnumerator Reload(){yield break;}
    virtual public IEnumerator EmptyReload() { yield break; }

    public IEnumerator Flash(float time)
    {
        foreach(var flash in muzzleFlashes)
        {
            flash.Play();
        }
        foreach(var light in muzzleFlashLights)
        {
            light.enabled = true;
        }
        yield return new WaitForSeconds(time);
        foreach(var light in muzzleFlashLights)
        {
            light.enabled = false;
        }
    }
}
