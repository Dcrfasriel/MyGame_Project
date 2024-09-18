using Assets.Frame.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameUIManager : MonoBehaviour,IUIManager
{
    #region Singleton

    private static GameUIManager uiManeger;

    public static GameUIManager GetInstance() { return uiManeger; }

    #endregion

    #region GlobalEvent
    private AudioSource ButtonClickAudio;
    private AudioSource ButtonSelectAndHoverSound;
    private AudioSource SliderSound;

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

    public void SetControlMute(bool IsMute)
    {
        ButtonClickAudio.mute=IsMute;
        ButtonSelectAndHoverSound.mute=IsMute;
        SliderSound.mute=IsMute;
    }

    #endregion

    [Header("±Ì≤„UI")]
    public SurfaceUI SurfaceUI;
    [Header("≤Àµ•UI")]
    public GameMenuPanel MenuUI;

    public KeyCode OpenMenuKey;

    public GameObject GreyMask;


    public Stack<IUIPanel> UIStack=new Stack<IUIPanel>();
    private bool IsMenuOpened = false;
    private AudioSource MenuOpenSound;
    private AudioSource MenuCloseSound;
    private void Awake()
    {
        ButtonClickAudio = transform.Find("ButClickSound").GetComponent<AudioSource>();
        ButtonSelectAndHoverSound = transform.Find("ButSelectAndHoverSound").GetComponent<AudioSource>();
        SliderSound = transform.Find("SliderSound").GetComponent<AudioSource>();
        uiManeger = this;
        CoverUIPanel(SurfaceUI);
        MenuOpenSound=transform.Find("MenuOpenSound").GetComponent<AudioSource>();
        MenuCloseSound=transform.Find("MenuCloseSound").GetComponent<AudioSource>();
    }

    private void CoverUIPanel(IUIPanel panel)
    {
        if(UIStack.Count > 0)
        {
            UIStack.Peek().SetAble(false);
            UIStack.Peek().GetEventGroup().IsUpdating = false;
        }
        UIStack.Push(panel);
        panel.GetEventGroup().IsUpdating = true;
        panel.SetAble(true);
    }

    private void ClosePeekPanel()
    {
        if (UIStack.Count > 0)
        {
            IUIPanel ui = UIStack.Pop();
            ui.SetAble(false);
            ui.GetEventGroup().IsUpdating=false;
        }
        if (UIStack.Count > 0)
        {
            IUIPanel ui = UIStack.Peek();
            ui.SetAble(true);
            ui.GetEventGroup().IsUpdating=true;
        }
    }

    public void AddEventToSurfaceUI(Action<List<KeyCode>, bool> action,params KeyCode[] keys)
    {
        SurfaceUI.GetEventGroup().AddEvent(action, keys);
    }

    public void AddEventToSurfaceUI(Action<List<string>, bool> action, params string[] keys)
    {
        SurfaceUI.GetEventGroup().AddEvent(action, keys);
    }

    private void CheckInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (EventSystem.current.currentSelectedGameObject == null && (Mathf.Abs(x) > 0.1 || Mathf.Abs(y) > 0.1))
        {
            if (UIStack.Count > 0)
            {
                UIStack.Peek().FirstSelect();
            }
        }
    }

    private void Update()
    {
        CheckInput();
        if (UIStack.Count > 0)
        {
            foreach(var ui in UIStack)
            {
                ui.GetEventGroup().Update();
            }
        }
        if (Input.GetKeyDown(OpenMenuKey))
        {
            TabMenu();
        }
    }

    public void TabMenu()
    {
        if (IsMenuOpened)
        {
            MenuCloseSound.Play();
            Cursor.lockState = CursorLockMode.Locked;
            ClosePeekPanel();
            IsMenuOpened = false;
            GreyMask.SetActive(false);
        }
        else
        {
            MenuOpenSound.Play();
            Cursor.lockState = CursorLockMode.None;
            CoverUIPanel(MenuUI);
            IsMenuOpened = true;
            GreyMask.SetActive(true);
        }
    }
}
