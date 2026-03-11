using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
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

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        muzzleFlashes = GetComponentsInChildren<ParticleSystem>();
        muzzleFlashLights = GetComponentsInChildren<Light>();
    }

    virtual public IEnumerator Attack(){yield break;}
    virtual public IEnumerator Move(){yield break;}

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
