using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeltRevolverAnimScript : Actions
{
    override public IEnumerator Fire()
    {
        anim.SetTrigger("TrFire");
        foreach(var animator in GetComponentsInChildren<Animator>())
        {
            animator.SetTrigger("TrFire");
        }
        foreach(var casing in GetComponentsInChildren<AddCasing>())
        {
            casing.AddLink();
        }
        yield return new WaitForSeconds(fireSfxDelay);
        source.PlayOneShot(shotSound);
        yield break;
    }
}
