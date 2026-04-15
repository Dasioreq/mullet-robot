using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// @class SwitchMusic
/// @brief Script for switching between intermission and combat music
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

    /// @brief Transitions between the intermission music track and one, randomly selected combat track.
    /// To create a fade-in-out effect, a second, temporary AudioSource is created and its volume gets linearly interpolated to 0
    /// 
    /// @param intermission whether to switch to or from the intermission track
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
