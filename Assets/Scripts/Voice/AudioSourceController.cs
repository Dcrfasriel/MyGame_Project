using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    public bool IsEffect;

    private AudioSource audioSource;
    private SettingsLoader settingsLoader;
    public float FVolume { get; set; }
    public float RVolume { get; set; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        FVolume=audioSource.volume;
        RVolume = FVolume;
    }
    private void Start()
    {
        settingsLoader = SettingsLoader.GetInstance();
    }
    private void Update()
    {
        if (IsEffect)
        {
            RVolume = FVolume * settingsLoader.CurrentSettings.VoiceSetting.EffectVolume;
        }
        else
        {
            RVolume = FVolume * settingsLoader.CurrentSettings.VoiceSetting.BackGroundVolume;
        }
        audioSource.volume = RVolume;
    }

    public void SetFVolume(float value)
    {
        FVolume = value;
    }
}
