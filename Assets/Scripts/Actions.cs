using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Actions : MonoBehaviour
{
    [SerializeField] protected AudioClip shotSound;
    [SerializeField] protected AudioClip reloadSound;
    [SerializeField] protected float fireSfxDelay;
    [SerializeField] protected float reloadSfxDelay;

    protected Animator anim;
    protected AudioSource source;

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    virtual public IEnumerator Fire(){yield break;}
    virtual public IEnumerator Reload(){yield break;}
}
