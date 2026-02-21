using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LongRevolverAnimScript : Actions
{
    [SerializeField] AudioClip altReloadSound;

    override public IEnumerator Fire()
    {
        anim.SetTrigger("TrFire");
        yield return new WaitForSeconds(fireSfxDelay);
        source.PlayOneShot(shotSound);
        yield break;
    }

    public override IEnumerator Reload()
    {
        anim.SetTrigger("TrReload");
        yield return new WaitForSeconds(reloadSfxDelay);
        int chance = Random.Range(0, 100);
        if(chance < 20)
        {
            source.PlayOneShot(altReloadSound);
        }
        else
        {
            source.PlayOneShot(reloadSound);
        }
        yield break;
    }
}
