using System.Collections;
using UnityEngine;
using static GameController;

public class IntermisionActions : MonoBehaviour
{
    Rigidbody rb;
    SwitchMusic jukebox;

    public bool endLevel = false;
    private bool cardsTriggered = false;
    public GeneratorBehaviour levelGenerator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jukebox = GameObject.FindWithTag("Music").GetComponent<SwitchMusic>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(!endLevel)
            if(other.gameObject.tag == "MainCamera")
        {
                StartCoroutine(jukebox.Switch(false));
            if (gameController.level > 0 && !cardsTriggered) 
            {
                Cards cards = Object.FindAnyObjectByType<Cards>();
                cards.StartCoroutine(cards.WaitAndShowCards());
                cardsTriggered = true;
            }      
        }
            
        if(other.gameObject.tag == "MainCamera")
            if(endLevel)
            {
                StartCoroutine(RegenerateLevel(other, 1f));
            }
    }

    public void ManualMusicSwitch()
    {
        StartCoroutine(jukebox.Switch(false));
    }

    void OnTriggerExit(Collider other)
    {
        if(!endLevel)
            if(other.gameObject.tag == "MainCamera")
                StartCoroutine(jukebox.Switch(true));
    }

    IEnumerator RegenerateLevel(Collider other, float time)
    {
        foreach(var anim in GetComponentsInChildren<Animator>())
        {
            anim.SetTrigger("Tr");
        }

        float elapsed = 0;

        while(elapsed < time)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameController.GenerateLevel();

        var player = GameObject.FindWithTag("Player");

        if(player)
        {
            player.transform.position = Quaternion.Inverse(transform.rotation) * (player.transform.position - transform.position) + new Vector3(0, 0, -7.5f);
            gameController.level++;
            player.GetComponent<Rigidbody>().linearVelocity = Quaternion.Inverse(transform.rotation) * player.GetComponent<Rigidbody>().linearVelocity;
            player.GetComponent<Rigidbody>().MovePosition(Quaternion.Inverse(transform.rotation) * (player.transform.position - transform.position) + new Vector3(0, 0, -7.5f));
        }

        other.gameObject.GetComponent<CameraContoller>().SetRotation(Quaternion.Inverse(transform.rotation) * other.transform.rotation);
        yield break;
    }
}
