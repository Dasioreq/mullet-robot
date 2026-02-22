using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FatShotgunAnimScript : Actions
{
    override public IEnumerator Fire()
    {
        anim.SetTrigger("TrFire");
        yield return new WaitForSeconds(fireSfxDelay);
        source.PlayOneShot(shotSound);
        yield break;
    }
}
