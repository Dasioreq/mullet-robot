using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Unity.Mathematics;
using static UnityEngine.Rendering.DebugUI;

public class MasterVolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider MasterSlider;
    private string musicParam = "Sfx";
    public TMP_Text text;

    private void Start()
    {
        MasterSlider.onValueChanged.AddListener(UpdateVolumeFromSlider);
    }
    public void Update()
    {
        UpdateVolumeFromSlider(MasterSlider.value);
    }

    public void UpdateVolumeFromSlider(float percent)
    {
        float value = Mathf.Max(percent / 100f, 0.0001f);
        float dB = Mathf.Log10(value) * 20f;
        mixer.SetFloat(musicParam, dB);
        text.text = math.round(value * 100).ToString();
    }
}
