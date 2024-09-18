using Assets.Frame.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceSettingGroup : MonoBehaviour,ISettingPanel
{
    private SettingsLoader settingsLoader;

    public Slider EffectSlider;

    public Slider BackgroundSlider;
    private void Start()
    {
        settingsLoader = SettingsLoader.GetInstance();
        EffectSlider.GetComponent<SliderController>().IsPlaySound = false;
        BackgroundSlider.GetComponent<SliderController>().IsPlaySound = false;
        EffectSlider.value = settingsLoader.CurrentSettings.VoiceSetting.EffectVolume;
        BackgroundSlider.value = settingsLoader.CurrentSettings.VoiceSetting.BackGroundVolume;
        StartCoroutine("SetPlayToTrue");
    }

    private IEnumerator SetPlayToTrue()
    {
        yield return new WaitForSeconds(1);
        EffectSlider.GetComponent<SliderController>().IsPlaySound =true;
        BackgroundSlider.GetComponent<SliderController>().IsPlaySound = true;
    }

    public void OnEffectValueChange()
    {
        settingsLoader.CurrentSettings.VoiceSetting.EffectVolume = EffectSlider.value;
    }

    public void OnBackgroundValueChange()
    {
        settingsLoader.CurrentSettings.VoiceSetting.BackGroundVolume = BackgroundSlider.value;
    }

    public void RecoverToDefault()
    {
        float effvalue = settingsLoader.DefaultSettings.VoiceSetting.EffectVolume;
        float backvalue = settingsLoader.DefaultSettings.VoiceSetting.BackGroundVolume;
        EffectSlider.GetComponent<SliderController>().IsPlaySound = false;
        BackgroundSlider.GetComponent<SliderController>().IsPlaySound = false;
        EffectSlider.value=effvalue;
        BackgroundSlider.value=backvalue;
        StartCoroutine("SetPlayToTrue");
    }
}
