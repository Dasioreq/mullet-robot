using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeltRevolverAnimScript : MonoBehaviour
{
    private Animator anim;
    private bool isOn = false;
    [SerializeField] private float fireOffset;
    [SerializeField] private AudioClip shotSound;
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
}
