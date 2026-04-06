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
                gameController.GenerateLevel();

                var player = GameObject.FindWithTag("Player");

                if(player)
                {
                    player.transform.position = Quaternion.Inverse(transform.rotation) * (player.transform.position - transform.position) + new Vector3(0, 0, -7.5f);
                    gameController.level++;
                    player.GetComponent<Rigidbody>().linearVelocity = Quaternion.Inverse(transform.rotation) * player.GetComponent<Rigidbody>().linearVelocity;
                }

                other.gameObject.GetComponent<CameraContoller>().SetRotation(Quaternion.Inverse(transform.rotation) * other.transform.rotation);
            }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "MainCamera")
            StartCoroutine(jukebox.Switch(true));
    }
}
