using Assets.Frame.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettingGroup : MonoBehaviour,ISettingPanel
{
    public Toggle DisplayToggle;

    private SettingsLoader settingsLoader;

    private void Start()
    {
        settingsLoader=SettingsLoader.GetInstance();
        DisplayToggle.isOn = settingsLoader.CurrentSettings.ControlSetting.IsDisplayAtBeginning;
        StartCoroutine("SetUIReticentForPeriod");
    }
    private IEnumerator SetUIReticentForPeriod()
    {
        DisplayToggle.gameObject.GetComponent<ToggleController>().IsPlay = false;
        yield return new WaitForSeconds(0.5f);
        DisplayToggle.gameObject.GetComponent<ToggleController>().IsPlay = true;
    }
    public void OnDisplayValueChange()
    {
        settingsLoader.CurrentSettings.ControlSetting.IsDisplayAtBeginning=DisplayToggle.isOn;
    }

    public void RecoverToDefault()
    {
        DisplayToggle.isOn= settingsLoader.DefaultSettings.ControlSetting.IsDisplayAtBeginning;
        StartCoroutine("SetUIReticentForPeriod");
    }
}
