using Assets.Frame.Interface;
using Assets.Frame.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SurfaceUI : MonoBehaviour,IUIPanel
{
    private GameObject JetpackPowerSlider;
    private Slider slider;
    private Text KeyCodeMonitor;
    private EventGroup eventGroup=new EventGroup();
    private CharacterEventCenter eventCenter;
    private void Awake()
    {
        JetpackPowerSlider = transform.Find("JetpackPowerSlider").gameObject;
        slider =transform.Find("JetpackPowerSlider/Slider").GetComponent<Slider>();
        KeyCodeMonitor = transform.Find("KeyCodeMonitor").GetComponent<Text>();
    }
    private void Start()
    {
        eventCenter=CharacterEventCenter.GetInstance();
    }
    public void SetPowerSliderValue(float value)
    {
        slider.value = value;
    }

    public void SetKeyCodeMonitor(bool IsActive,string Text)
    {
        KeyCodeMonitor.gameObject.SetActive(IsActive);
        KeyCodeMonitor.text=Text;
    }

    public EventGroup GetEventGroup()
    {
        return eventGroup;
    }

    public void SetAble(bool able)
    {
        //gameObject.SetActive(able);
    }

    #region Debug

    private void OnGUI()
    {
        if (DebugCenter.GetInstance().IsDebugUIActive)
        {
            GUILayout.BeginArea(new Rect(0, 20, 500, 500));

            GUILayout.Label("Player Velocity:" + CharacterEventCenter.GetInstance().GetPlayerRigibody().velocity.ToString() + " Magnitude:" + CharacterEventCenter.GetInstance().GetPlayerRigibody().velocity.magnitude.ToString());

            GUILayout.Label("Character State:"+CharacterEventCenter.GetInstance().CurrentState.ToString());

            GUILayout.EndArea();
        }
        if(eventCenter.CurrentState==CharacterState.Normal)
        {
            JetpackPowerSlider.SetActive(true);
        }
        else if (eventCenter.CurrentState == CharacterState.OnSpaceShip)
        {
            JetpackPowerSlider.SetActive(false);
        }
    }

    public void FirstSelect()
    {
        return;
    }

    #endregion
}
