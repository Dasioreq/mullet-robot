using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]

public class RifleMagAnimScript : Actions
{
    [SerializeField] AudioClip emptyReloadSound;
    [SerializeField] AudioClip altReloadSound;
    [SerializeField] AudioClip shotSound2;
    [SerializeField] AudioClip reloadSound2;
    [SerializeField] AudioClip Charginghandle;
    [SerializeField] AudioClip Charginghandle2;
    [SerializeField] float nextReloadSfxDelay;
    [SerializeField] float nextReloadSfxDelay2;
    [SerializeField] float nextReloadSfxDelay3;
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

        var casingScript = GetComponent<Casings>();
        casingScript.SpawnCasing(transform, transform.rotation * Quaternion.Euler(-90, 0, 0));

        yield return new WaitForSeconds(fireSfxDelay);
        int chance = Random.Range(0,2);
        if (chance < 1)
        {
            source.PlayOneShot(shotSound);
        }
        else
        {
            source.PlayOneShot(shotSound2);
        }
        yield break;
    }
    public override IEnumerator Reload()
    {
        anim.SetTrigger("TrReload");
        yield return new WaitForSeconds(reloadSfxDelay);
        source.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(nextReloadSfxDelay);
        source.PlayOneShot(reloadSound2);
        yield return new WaitForSeconds(nextReloadSfxDelay2);
        source.PlayOneShot(Charginghandle);
        yield return new WaitForSeconds(nextReloadSfxDelay3);
        int chance = Random.Range(0, 10);
        if (chance < 3)
        {
            source.PlayOneShot(altReloadSound);
        }
        else
        {
            source.PlayOneShot(Charginghandle2);
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
