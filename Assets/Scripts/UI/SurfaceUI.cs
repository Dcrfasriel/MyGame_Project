using Assets.Frame.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SurfaceUI : MonoBehaviour,IUIPanel
{
    private Slider slider;
    private void Awake()
    {
        slider=transform.Find("JetpackPowerSlider/Slider").GetComponent<Slider>();
    }

    public void SetPowerSliderValue(float value)
    {
        slider.value = value;
    }
}
