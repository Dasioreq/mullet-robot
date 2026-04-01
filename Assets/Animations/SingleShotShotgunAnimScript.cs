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
    [SerializeField] float nextReloadSfxDelay2;
    [SerializeField] AudioClip emptyReloadSound;
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
        anim.SetTrigger("TrReload");
        yield return new WaitForSeconds(reloadSfxDelay);
        source.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(nextReloadSfxDelay2);
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
