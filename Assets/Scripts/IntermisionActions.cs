using UnityEngine;

public class IntermisionActions : MonoBehaviour
{
    Rigidbody rb;
    SwitchMusic jukebox;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jukebox = GameObject.FindWithTag("Music").GetComponent<SwitchMusic>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "MainCamera")
            StartCoroutine(jukebox.Switch(false));
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "MainCamera")
            StartCoroutine(jukebox.Switch(true));
    }
}
