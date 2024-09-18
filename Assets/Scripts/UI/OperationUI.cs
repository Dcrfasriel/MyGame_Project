using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationUI : MonoBehaviour
{
    public OperationUIController CharacterUI;

    public OperationUIController SpaceShipUI;

    private OperationUIController CurrentUI=null;
    private SettingsLoader settingsLoader;
    private CharacterEventCenter characterEventCenter;
    private bool HasCharacterDisplayed=false;
    private bool HasSpaceShipDisplayed=false;

    private void Awake()
    {
        CharacterUI.gameObject.SetActive(true);
        SpaceShipUI.gameObject.SetActive(true);
    }
    private void Start()
    {
        GameUIManager.GetInstance().AddEventToSurfaceUI((keys, u) =>
        {
            if (u && Input.GetKeyDown(keys[0]))
            {
                if (CurrentUI != null)
                {
                    CurrentUI.SetDisplay(false);
                }
            }
        }, KeyCode.Tab);
        characterEventCenter = CharacterEventCenter.GetInstance();
        settingsLoader =SettingsLoader.GetInstance();
        if (settingsLoader.CurrentSettings.ControlSetting.IsDisplayAtBeginning&&!HasCharacterDisplayed)
        {
            SetDisplay(CharacterUI);
            HasCharacterDisplayed = true;
        }
    }

    private void SetDisplay(OperationUIController ui)
    {
        if (CurrentUI != null)
        {
            CurrentUI.SetDisplay(false);
        }
        CurrentUI = ui;
        ui.SetDisplay(true);
    }

    private void Update()
    {
        if(!HasSpaceShipDisplayed&& settingsLoader.CurrentSettings.ControlSetting.IsDisplayAtBeginning&&characterEventCenter.CurrentState==CharacterState.OnSpaceShip)
        {
            SetDisplay(SpaceShipUI);
            HasSpaceShipDisplayed = true;
        }
    }
}
