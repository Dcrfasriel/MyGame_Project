using Assets.Frame.Interface;
using Assets.Frame.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour, IUIPanel
{
    [Header("能否点击")]
    public bool CanClick;
    [Header("按钮单体移动时间")]
    public float ButtonMovingFullTime;
    [Header("按钮移动初速度")]
    public float ButtonMovingV0;
    [Header("按钮移动间隔时间")]
    public float ButtonMovingInterval;
    [Header("按钮消失移动附加参数")]
    public float ButtonDisappearParam;
    [Header("标题加载参数初值")]
    public float TitleStartValue;
    [Header("标题加载速度")]
    public float TitleLoadSpeed;
    [Header("标题加载参数最大值")]
    public float TitleMaxValue;
    [Header("设置Panel")]
    public SettingsPanel settingsPanel;

    private EventGroup eventGroup=new EventGroup();
    private MainMenuManager mainMenuManager;

    private ButtonGroup Group1;
    private ButtonGroup Group2;
    private ButtonGroupsTree groupsTree;
    private Text Title;
    private bool IsLoadTitle=false;
    private TemporaryEvent temporaryEvent=new TemporaryEvent();

    private void Start()
    {
        mainMenuManager=MainMenuManager.GetInstance();

        foreach(var g in new List<string>(){ "Group1", "Group2" }.Select(p => transform.Find(p).gameObject))
        {
            g.SetActive(true);
        }

        Title=transform.Find("Title").GetComponent<Text>();
        Group1=new ButtonGroup(new List<string>() { "StartGameBut", "OptionsBut","ExitBut" }.Select(p => transform.Find("Group1/" + p).gameObject).ToList(), ButtonMovingInterval,-120,168,ButtonMovingFullTime,ButtonMovingV0, ButtonDisappearParam);
        Group2 = new ButtonGroup(new List<string>() { "GeneralBut","ControlBut", "RenderBut", "VoiceBut", "BackBut1"  }.Select(p => transform.Find("Group2/" + p).gameObject).ToList(), 
            ButtonMovingInterval, -120, 168, ButtonMovingFullTime, ButtonMovingV0,ButtonDisappearParam);

        groupsTree = new ButtonGroupsTree(Group1,Group2);
        groupsTree.SetAllActive(true);
        Title.material.SetFloat("_RenderValue", TitleStartValue);
        mainMenuManager.UIPanel = this;
        #region Title

        mainMenuManager.AddObserver("MaskFirstDisappear", (b) =>
        {
            if (b)
            {
                IsLoadTitle = true;
                return true;
            }
            return false;
        });

        mainMenuManager.AddCondtionCheck("TitleLoadComplate", () =>
        {
            float value = Title.material.GetFloat("_RenderValue");
            int k = (TitleLoadSpeed <= 0) ? -1 : 1;
            return k * value >=k*TitleMaxValue;
        },true);

        #endregion

        mainMenuManager.AddObserver("TitleLoadComplate", (b) =>
        {
            if (b)
            {
                IsLoadTitle = false;
                groupsTree.ActiveGroup(0);
                return true;
            }
            return false;
        });

        
    }
    private void Update()
    {
        if(IsLoadTitle)
        {
            LoadTitle();
        }
        groupsTree.Update();
        temporaryEvent.CheckUpdate();
    }
    public void FirstSelect()
    {
        groupsTree.Select(0);
    }
    public EventGroup GetEventGroup()
    {
        return eventGroup;
    }

    public void SetAble(bool able)
    {
        this.enabled = able;
    }
    public void OnStartButtonClick()
    {
        if (CanClick)
        {
            mainMenuManager.OnButtonClick();
            mainMenuManager.SetCondition("BeginLoad", true);
        }
    }
    public void OnOptionsButtonClick()
    {
        if (CanClick)
        {
            groupsTree.ActiveGroup(1);
            mainMenuManager.OnButtonClick();
        }
    }

    public void OnExitButtonCLick()
    {
        if (CanClick)
        {
            mainMenuManager.OnButtonClick();
            Invoke("ExitGame", 1);
        }
    }
    private void ExitGame()
    {
        Application.Quit();
    }

    public void OnBack1ButtonClick()
    {
        if(CanClick)
        {
            mainMenuManager.OnButtonClick();
            groupsTree.ActiveGroup(0);
        }
    }

    public void OnVoiceButtonClick()
    {
        if (CanClick)
        {
            mainMenuManager.OnButtonClick();
            groupsTree.ActiveGroup(-1);
            settingsPanel.gameObject.SetActive(true);
            temporaryEvent.StartEvent(() =>
            {
                settingsPanel.SetActive(true,"VoiceGroup");
            }, () =>
            {
                return groupsTree.AppearIndexGroup == -1;
            });
            mainMenuManager.UIPanel = settingsPanel;
        }
    }

    public void OnControlButtonClick()
    {
        if (CanClick)
        {
            mainMenuManager.OnButtonClick();
            groupsTree.ActiveGroup(-1);
            settingsPanel.gameObject.SetActive(true);
            temporaryEvent.StartEvent(() =>
            {
                settingsPanel.SetActive(true, "ControlGroup");
            }, () =>
            {
                return groupsTree.AppearIndexGroup == -1;
            });
            mainMenuManager.UIPanel = settingsPanel;
        }
    }

    public void OnRenderButtonClick()
    {
        if (CanClick)
        {
            mainMenuManager.OnButtonClick();
            groupsTree.ActiveGroup(-1);
            settingsPanel.gameObject.SetActive(true);
            temporaryEvent.StartEvent(() =>
            {
                settingsPanel.SetActive(true, "RenderGroup");
            }, () =>
            {
                return groupsTree.AppearIndexGroup == -1;
            });
            mainMenuManager.UIPanel = settingsPanel;
        }
    }
    public void OnGeneralButtonClick()
    {
        if (CanClick)
        {
            mainMenuManager.OnButtonClick();
            groupsTree.ActiveGroup(-1);
            settingsPanel.gameObject.SetActive(true);
            temporaryEvent.StartEvent(() =>
            {
                settingsPanel.SetActive(true, "GeneralGroup");
            }, () =>
            {
                return groupsTree.AppearIndexGroup == -1;
            });
            mainMenuManager.UIPanel = settingsPanel;
        }
    }
    private void LoadTitle()
    {
        float value = Title.material.GetFloat("_RenderValue");
        int k = (TitleLoadSpeed <= 0) ? -1 : 1;
        if(k*value<k*TitleMaxValue)
        {
            value += TitleLoadSpeed * Time.deltaTime;
            if (k*value > k*TitleMaxValue) value = TitleMaxValue;
            Title.material.SetFloat("_RenderValue", value);
        }
    }

    public void SetButActive(int index)
    {
        groupsTree.ActiveGroup(index);
    }

}
