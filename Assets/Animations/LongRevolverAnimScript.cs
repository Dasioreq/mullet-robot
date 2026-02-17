using Unity.VisualScripting;
using UnityEngine;

public class LongRevolverAnimScript : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(anim != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("TrFire");
            }

            if(Input.GetKeyDown("r"))
            {
                anim.SetTrigger("TrReload");
            }
        }
    }
}
