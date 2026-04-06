using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeltRevolverAnimScript : Actions
{
    [SerializeField] private ParticleSystem[] chargeObjects;
    [SerializeField] float smokeTimey;
    override public IEnumerator Fire()
    {
        anim.SetTrigger("TrFire");
        StartCoroutine(Flash(.1f));
        foreach(var animator in GetComponentsInChildren<Animator>())
        {
            chargeObjects[0].Play();
            chargeObjects[1].Play();
            chargeObjects[2].Play();
            chargeObjects[3].Play();
            int chance2 = Random.Range(0, 2);
            if (chance2 < 1)
            {
                chargeObjects[4].Play();
                chargeObjects[5].Play();
            }           
            animator.SetTrigger("TrFire");
            
        }
        foreach(var casing in GetComponentsInChildren<AddCasing>())
        {
            casing.AddLink();
        }
        yield return new WaitForSeconds(fireSfxDelay);
        source.PlayOneShot(shotSound);
        yield return new WaitForSeconds(smokeTimey);
        chargeObjects[4].Stop();
        chargeObjects[5].Stop();
        yield break;
    }
}
