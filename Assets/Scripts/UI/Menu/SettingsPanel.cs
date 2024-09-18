using Assets.Frame.Interface;
using Assets.Frame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour,IUIPanel
{
    private RectTransform rectTransform;
    [Header("出现&消失速度")]
    public float Speed;
    [Header("是否激活")]
    public bool IsActive;

    public StartUI startUI;
    private EventGroup eventGroup=new EventGroup();
    private SettingGroupBase CurrentSettingGroup;
    private ISettingPanel CurrentPanel;
    private List<string> PublicControlList = new List<string>()
    {
        "BackBut",
        "DefaultBut"
    };

    private MainMenuManager mainMenuManager;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
    }
    private void Start()
    {
        mainMenuManager =MainMenuManager.GetInstance();
        OnActive += (b) =>
        {
            if (!b)
            {
                startUI.SetButActive(1);
                this.gameObject.SetActive(false);
            }
        };
    }

    private void Update()
    {
        if(IsActive)
        {
            if (rectTransform.localScale.x < 0.99)
            {
                rectTransform.localScale += Speed * Time.deltaTime * Vector3.one;
            }
            else
            {
                rectTransform.localScale = Vector3.one;
            }
        }
        else
        {
            if (rectTransform.localScale.x >0.01)
            {
                rectTransform.localScale -= Speed * Time.deltaTime * Vector3.one;
            }
            else
            {
                rectTransform.localScale = Vector3.zero;
            }
        }
        CheckEvent();
    }

    public void SetActive(bool value,string InvokeGroupName="")
    {
        WillCheckEvent = true;
        IsActive = value;
        for(int i=0;i<transform.childCount;i++)
        {
            if(transform.GetChild(i).name == InvokeGroupName|| PublicControlList.Contains(transform.GetChild(i).name))
            {
                transform.GetChild(i).gameObject.SetActive(true);
                if(transform.GetChild(i).name == InvokeGroupName)
                {
                    CurrentSettingGroup = transform.GetChild(i).GetComponent<SettingGroupBase>();
                    CurrentPanel = transform.GetChild(i).GetComponent<ISettingPanel>();
                }
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public event Action<bool> OnActive;
    private bool WillCheckEvent = false;
    private void CheckEvent()
    {
        if (WillCheckEvent)
        {
            if (IsActive && rectTransform.localScale.x > 0.99)
            {
                WillCheckEvent = false;
                OnActive?.Invoke(true);
            }
            else if(!IsActive && rectTransform.localScale.x < 0.01)
            {
                WillCheckEvent = false;
                OnActive?.Invoke(false);
            }
        }
    }

    public void OnBackButClick()
    {
        mainMenuManager.OnButtonClick();
        SetActive(false);
        mainMenuManager.UIPanel = startUI;
    }

    public void OnDefaultButClick()
    {
        mainMenuManager.OnButtonClick();
        CurrentPanel?.RecoverToDefault();
    }

    public EventGroup GetEventGroup()
    {
        return eventGroup;
    }

    public void SetAble(bool able)
    {
        SetActive(able);
    }

    public void FirstSelect()
    {
        if(CurrentSettingGroup != null)
        {
            CurrentSettingGroup.FirstSelect();
        }
        return;
    }

    public bool IsLoadComplete()
    {
        return rectTransform.localScale.x > 0.9999;
    }
}
