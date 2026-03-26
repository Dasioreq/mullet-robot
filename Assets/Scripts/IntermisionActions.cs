using UnityEngine;

public class IntermisionActions : MonoBehaviour
{
    Rigidbody rb;
    SwitchMusic jukebox;

    bool endLevel = false;
    GeneratorBehaviour levelGenerator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jukebox = GameObject.FindWithTag("Music").GetComponent<SwitchMusic>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "MainCamera")
            StartCoroutine(jukebox.Switch(false));

        if(endLevel)
        {
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "MainCamera")
            StartCoroutine(jukebox.Switch(true));
    }
}
