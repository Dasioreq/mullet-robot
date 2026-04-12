using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]

public class RifleBeltAnimScript : Actions
{
    [SerializeField] AudioClip shotSound2;
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
        StartCoroutine(Flash(.05f));

        var casingScript = GetComponent<Casings>();
        casingScript.SpawnCasing(transform, transform.rotation * Quaternion.Euler(-90, 0, 0));

        foreach(var animator in GetComponentsInChildren<Animator>())
        {
            animator.SetTrigger("TrFire");
        }

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
        yield break;
    }
    public override IEnumerator EmptyReload()
    {
        StartCoroutine(Reload());
        yield break;
    }
}
