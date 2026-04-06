using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FatShotgunAnimScript : Actions
{
    [SerializeField] AudioClip reloadSound2;
    [SerializeField] AudioClip reloadSound3;
    [SerializeField] AudioClip reloadSound4;
    [SerializeField] AudioClip altReloadSound;
    [SerializeField] float nextReloadSfxDelay;
    [SerializeField] float nextReloadSfxDelay2;
    [SerializeField] float nextReloadSfxDelay3;
    [SerializeField] private ParticleSystem[] chargeObjects;
    override public IEnumerator Fire()
    {
        anim.SetTrigger("TrFire");
        StartCoroutine(Flash(.1f));
        yield return new WaitForSeconds(fireSfxDelay);
        source.PlayOneShot(shotSound);
        yield return new WaitForSeconds(reloadSfxDelay);
        source.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(nextReloadSfxDelay2);
        source.PlayOneShot(reloadSound3);
        yield return new WaitForSeconds(nextReloadSfxDelay3);
        source.PlayOneShot(reloadSound4);
        yield return new WaitForSeconds(nextReloadSfxDelay);
        chargeObjects[0].Play();
        chargeObjects[1].Play();
        chargeObjects[2].Play();
        int chance = Random.Range(0, 100);
        if (chance < 15)
        {
            source.PlayOneShot(altReloadSound);
        }
        else
        {
            source.PlayOneShot(reloadSound2);
        }
        yield break;
    }
}
