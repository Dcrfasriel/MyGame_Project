using Assets.Frame.DAL;
using Assets.Frame.DAL.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    
    void Start()
    {
        VoiceSettings vsettings = new VoiceSettings();
        vsettings.EffectVolume = 0.5f;
        vsettings.BackGroundVolume = 0.5f;
        ControlSettings controlsettings = new ControlSettings();
        controlsettings.IsDisplayAtBeginning = true;
        Settings settings = new Settings(Name: "Current", voiceSettings: vsettings,controlSettings:controlsettings);
        Debug.Log(DataAccessor.SaveBaseInfo(settings.ToBaseInfo()));
    }
}
