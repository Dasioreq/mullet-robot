using System.Collections;
using UnityEngine;
using static GameController;

/// @class IntermisionActions
/// @brief Handles what happens when the player enters/exits the Intermission/Generator %Room
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

    /// @brief Plays the intermission music, regenerates the player's Lifetime and if the room is the level switch trigger - calls \ref IntermisionActions::RegenerateLevel
    public void OnTriggerEnter(Collider other)
    {
        if(!endLevel)
            if(other.gameObject.tag == "MainCamera")
            {
                gameController.intermission = true;
                StartCoroutine(jukebox.Switch(false));
                if (gameController.record < gameController.level)
                { 
                    gameController.record = gameController.level; 
                }
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
                gameController.intermission = true;
                StartCoroutine(RegenerateLevel(other, 1f));
            }
    }

    /// @brief A manual switch for the music
    public void ManualMusicSwitch()
    {
        StartCoroutine(jukebox.Switch(false));
    }

    /// @brief Picks a random combat music track
    public void OnTriggerExit(Collider other)
    {
        if(!endLevel)
            if(other.gameObject.tag == "MainCamera")
            {
                StartCoroutine(jukebox.Switch(true));
                gameController.intermission = false;
            }
    }

    /// @brief A coroutine that plays the room's door closing animation, repositions the player for a seamless transition and calls to the \ref GameController to regenerate the level (GeneratorBehaviour::Generate) 
    public IEnumerator RegenerateLevel(Collider other, float time)
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
            Physics.SyncTransforms();
        }

        other.gameObject.GetComponent<CameraContoller>().SetRotation(Quaternion.Inverse(transform.rotation) * other.transform.rotation);
        yield break;
    }
}
