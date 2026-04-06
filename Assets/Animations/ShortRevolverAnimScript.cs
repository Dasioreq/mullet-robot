using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShortRevolverAnimScript : Actions
{
    [SerializeField] AudioClip altReloadSound;
    [SerializeField] AudioClip emptyReloadSound;
    [SerializeField] private ParticleSystem[] chargeObjects;

    override public IEnumerator Fire()
    {
        int chance2 = Random.Range(0, 2);
        if (chance2 < 1)
        {
            chargeObjects[0].Play();
        }
        else
        {
            chargeObjects[1].Play();
        }
        anim.SetTrigger("TrFire");
        StartCoroutine(Flash(.1f));
        yield return new WaitForSeconds(fireSfxDelay);
        source.PlayOneShot(shotSound);
        yield break;
    }

    public override IEnumerator Reload()
    {
        anim.SetTrigger("TrReload");
        yield return new WaitForSeconds(reloadSfxDelay);
        int chance = Random.Range(0, 100);
        if(chance < 30)
        {
            source.PlayOneShot(altReloadSound);
        }
        else
        {
            source.PlayOneShot(reloadSound);
        }
        yield break;
    }
    public override IEnumerator EmptyReload()
    {
        source.PlayOneShot(emptyReloadSound);
        StartCoroutine(Reload());
        yield break;
    }
}
