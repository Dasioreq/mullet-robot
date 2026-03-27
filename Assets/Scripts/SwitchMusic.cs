using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SwitchMusic : MonoBehaviour
{
    AudioSource mainAS;

    [SerializeField] float switchTime;

    [SerializeField] AudioClip intermissionTrack;
    [SerializeField] AudioClip[] combatTracks;

    [SerializeField] AudioMixerGroup musicMixerGroup;

    bool switchState = true;

    void Start()
    {
        mainAS = GetComponent<AudioSource>();
        mainAS.clip = intermissionTrack;

        if (musicMixerGroup != null)
            mainAS.outputAudioMixerGroup = musicMixerGroup; 
    }

    public IEnumerator Switch(bool intermission)
    {
        if(switchState == intermission)
            yield break;

        switchState = intermission;
        float timer = switchTime;

        AudioSource newAS = gameObject.AddComponent<AudioSource>();

        if (musicMixerGroup != null)
            newAS.outputAudioMixerGroup = musicMixerGroup; 

        if (intermission)
        {
            int randIndex = Random.Range(0, combatTracks.Length);
            newAS.clip = combatTracks[randIndex];
        }
        else
        {
            newAS.clip = intermissionTrack;
        }

        newAS.loop = true;
        newAS.Play();

        while (timer > 0)
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
}
