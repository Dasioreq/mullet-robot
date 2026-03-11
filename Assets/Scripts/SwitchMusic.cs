using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class SwitchMusic : MonoBehaviour
{
    AudioSource mainAS;
    [SerializeField] float switchTime;

    [SerializeField] AudioClip intermissionTrack;
    [SerializeField] AudioClip[] combatTracks;

    void Start()
    {
        mainAS = GetComponent<AudioSource>();
        mainAS.clip = intermissionTrack;
    }

    public IEnumerator Switch(bool intermission)
    {
        float timer = switchTime;

        if(intermission)
        {
            AudioSource newAS = gameObject.AddComponent<AudioSource>();

            int randIndex = Random.Range(0, combatTracks.Length);
            AudioClip clip = combatTracks[randIndex];

            newAS.clip = clip;
            newAS.loop = true;
            newAS.Play();

            while(timer > 0)
            {
                newAS.volume = (switchTime - timer) / switchTime;
                mainAS.volume = timer / switchTime;

                timer -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            Destroy(mainAS);

            mainAS = newAS;
            mainAS.volume = 1;
        }
        else
        {
            AudioSource newAS = gameObject.AddComponent<AudioSource>();

            AudioClip clip = intermissionTrack;

            newAS.clip = clip;
            newAS.loop = true;
            newAS.Play();

            while(timer > 0)
            {
                newAS.volume = (switchTime - timer) / switchTime;
                mainAS.volume = timer / switchTime;

                timer -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            Destroy(mainAS);

            mainAS = newAS;
            mainAS.volume = 1;
        }

        yield break;
    }
}
