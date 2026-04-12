using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SingleShotShotgunAnimScript : Actions
{
    [SerializeField] AudioClip reloadSound2;
    [SerializeField] AudioClip reloadSound3;
    [SerializeField] AudioClip altReloadSound;
    [SerializeField] float nextReloadSfxDelay;
    [SerializeField] float nextReloadSfxDelay3;
    [SerializeField] float nextReloadSfxDelay4;
    [SerializeField] float nextReloadSfxDelay5;
    [SerializeField] AudioClip emptyReloadSound;
    [SerializeField] private ParticleSystem[] chargeObjects;
    override public IEnumerator Fire()
    {
        anim.SetTrigger("TrFire");
        StartCoroutine(Flash(.1f));
        yield return new WaitForSeconds(fireSfxDelay);
        source.PlayOneShot(shotSound);
        yield break;
    }
    

    public override IEnumerator Reload()
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
        anim.SetTrigger("TrReload");
        yield return new WaitForSeconds(reloadSfxDelay);
        source.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(nextReloadSfxDelay5);
        chargeObjects[0].Play();
        chargeObjects[1].Play();
        yield return new WaitForSeconds(nextReloadSfxDelay3);
        chargeObjects[2].Play();
        chargeObjects[3].Play();
        yield return new WaitForSeconds(nextReloadSfxDelay4);
        chargeObjects[0].Stop();
        chargeObjects[1].Stop();
        chargeObjects[2].Stop();
        chargeObjects[3].Stop();
        source.PlayOneShot(reloadSound3);
        yield return new WaitForSeconds(nextReloadSfxDelay);
        int chance = Random.Range(0, 100);
        if (chance < 30)
        {
            source.PlayOneShot(altReloadSound);
        }
        else
        {
            source.PlayOneShot(reloadSound2);
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
