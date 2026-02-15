using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RevolverAnimScript : MonoBehaviour
{
    private Animator anim;
    private bool isOn = false;
    [SerializeField] private float fireOffset;
    [SerializeField] private float reloadOffset1;
    [SerializeField] private float reloadOffset2;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip alternatifeReloadSound;
    private AudioSource source;

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(anim != null && !isOn)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Fire());
            }

            if(Input.GetKeyDown("r"))
            {
                StartCoroutine(Reload());
            }
        }
    }
    IEnumerator Fire()
    {
        isOn = true;
        source.PlayOneShot(shotSound);
        anim.SetTrigger("TrFire");

        yield return new WaitForSeconds(fireOffset);

        isOn = false;
    }
    IEnumerator Reload()
    {
        isOn = true;
        anim.SetTrigger("TrReload");

        yield return new WaitForSeconds(reloadOffset1);
        int chance = Random.Range(0, 100);
        if (chance < 20)
        {
            source.PlayOneShot(alternatifeReloadSound);
        }
        else
        {
            source.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadOffset2);

        isOn = false;
    }
}
