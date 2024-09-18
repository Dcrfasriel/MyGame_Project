using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour, ISelectHandler
{
    public RectTransform BackGround;

    public Color SelectColor;

    public bool IsPlay;

    private MainMenuManager mainMenuManager;
    private Color OrgColor;
    void Start()
    {
        mainMenuManager = MainMenuManager.GetInstance();
        OrgColor = BackGround.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            BackGround.GetComponent<Image>().color = OrgColor;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        mainMenuManager.OnButtonHover();
        BackGround.GetComponent<Image>().color = SelectColor;
    }

    public void OnValueChange()
    {
        if (IsPlay && mainMenuManager != null)
        {
            mainMenuManager.OnToggleClick();
        }
    }
}
