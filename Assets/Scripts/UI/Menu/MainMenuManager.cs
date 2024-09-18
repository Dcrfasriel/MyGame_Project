using Assets.Frame.Interface;
using Assets.Frame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour,IUIManager
{
    [Header("»­²¼")]
    public Transform Canvas;

    #region Singleton

    private static MainMenuManager uiManeger;

    public static MainMenuManager GetInstance() { return uiManeger; }

    #endregion

    #region GlobalEvent
    private AudioSource ButtonClickAudio;
    private AudioSource ButtonSelectAndHoverSound;
    private AudioSource SliderSound;
    private AudioSource ToggleSound;

    public void OnButtonHover()
    {
        ButtonSelectAndHoverSound.Play();
    }
    public void OnButtonClick()
    {
        ButtonClickAudio.Play();
    }
    private bool WillSliderSoundPlay = true;
    private void SetSliderSoundAble()
    {
        WillSliderSoundPlay = true;
    }
    public void OnSliderMove()
    {
        if (WillSliderSoundPlay)
        {
            SliderSound.Play();
            WillSliderSoundPlay = false;
            Invoke("SetSliderSoundAble", 0.2f);
        }
    }

    public void OnToggleClick()
    {
        ToggleSound.Play();
    }
    #endregion

    #region Time

    public float InvokeTime;

    #endregion

    #region EventTable

    private ConditionObserver eventObserver;

    private void AddBaseEvent()
    {
        eventObserver = new ConditionObserver();
        eventObserver.SetEvent("MaskFirstDisappear", false,true);
        eventObserver.SetEvent("SurfaceUILoad", false,true);
        eventObserver.SetEvent("BeginLoad", false,false);
        eventObserver.SetEvent("BlackMaskCoverAfterLoad", false, true);
        eventObserver.SetEvent("TitleLoadComplate", false, true);
    }

    public bool GetCondition(string EventName)
    {
        return eventObserver.GetValue(EventName);
    }

    public void SetCondition(string EventName,bool Bool)
    {
        eventObserver.SetEvent(EventName, Bool);
        return;
    }

    public void AddObserver(string Name, Func<bool, bool> Observer)
    {
        eventObserver.AddObserver(Name, Observer);
    }

    public void AddCondtionCheck(string Name, Func<bool> func, bool IsCheckOneTime = false)
    {
        eventObserver.AddCondtionCheck(Name, func, IsCheckOneTime);
    }


    #endregion

    #region EventDetect
    private void SetBaseEvent()
    {
        #region BlackMaskEvent
        AddCondtionCheck("MaskFirstDisappear", () =>
        {
            return blackMaskController.MaskValue <= 0.1;
        }, true);

        AddObserver("BeginLoad", (b) =>
        {
            if (b)
            {
                blackMaskController.SetAppear(true);
                return true;
            }
            return false;
        });
        AddObserver("BeginLoad", (b) =>
        {
            if (b&& blackMaskController.MaskValue >= 1.99)
            {
                SetCondition("BlackMaskCoverAfterLoad", true);
                return true;
            }
            return false;
        });

        #endregion

        #region Load

        AddObserver("BlackMaskCoverAfterLoad", (b) =>
        {
            if (b)
            {
                StartCoroutine("StartLoadScene");
                return true;
            }
            return false;
        });
        #endregion
    }

    #endregion

    private IUIPanel uipanel;
    public IUIPanel UIPanel
    {
        get { return uipanel; }
        set
        {
            EventSystem.current.SetSelectedGameObject(null);
            uipanel = value;
        }
    }
    private BlackMaskController blackMaskController;
    private ProcessIndicatorController processIndicatorController;
    private void Awake()
    {
        AddBaseEvent();
        uiManeger = this;
        ButtonClickAudio=transform.Find("ButClickSound").GetComponent<AudioSource>();
        ButtonSelectAndHoverSound= transform.Find("ButSelectAndHoverSound").GetComponent<AudioSource>();
        SliderSound = transform.Find("SliderSound").GetComponent<AudioSource>();
        ToggleSound= transform.Find("ToggleSound").GetComponent<AudioSource>();
        InvokeTime = 0;
        blackMaskController=Canvas.Find("BlackMask").GetComponent<BlackMaskController>();
        processIndicatorController=Canvas.Find("ProcessIndicator").GetComponent<ProcessIndicatorController>();
        SetBaseEvent();
    }

    private void Update()
    {
        InvokeTime += Time.deltaTime;
        eventObserver.Update();
        CheckInput();
    }

    private void CheckInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (EventSystem.current.currentSelectedGameObject == null && (Mathf.Abs(x) > 0.1 || Mathf.Abs(y) > 0.1))
        {
            if(UIPanel !=null) UIPanel.FirstSelect();
        }
    }

    private IEnumerator StartLoadScene()
    {
        processIndicatorController.SetActive(true);
        processIndicatorController.SetActualValue(0);
        yield return new WaitForSeconds(1);
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainGame");
        operation.allowSceneActivation = false;
        while (operation.progress<0.89)
        {
            processIndicatorController.SetSmoothValue(operation.progress/0.9f);
            yield return null;
        }
        processIndicatorController.SetSmoothValue(1);
        yield return new WaitForSeconds(1);
        processIndicatorController.SetActive(false);
        operation.allowSceneActivation=true;
        yield return null;
    }
}
