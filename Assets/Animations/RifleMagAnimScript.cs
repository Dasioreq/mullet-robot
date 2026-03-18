using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class RifleMagAnimScript : Actions
{
    override public IEnumerator Fire()
    {
        anim.SetTrigger("TrFire");
        StartCoroutine(Flash(.1f));

        var casingScript = GetComponent<Casings>();
        casingScript.SpawnCasing(transform, transform.rotation * Quaternion.Euler(-90, 0, 0));

        yield return new WaitForSeconds(fireSfxDelay);
        source.PlayOneShot(shotSound);
        yield break;
    }

    public override IEnumerator Reload()
    {
        anim.SetTrigger("TrReload");
        yield return new WaitForSeconds(reloadSfxDelay);
        source.PlayOneShot(reloadSound);
        yield break;
    }
    public override IEnumerator EmptyReload()
    {
        StartCoroutine(Reload());
        yield break;
    }
}
