using Assets.Frame.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderController : MonoBehaviour, ISelectHandler
{
    public RectTransform Mask;

    public RectTransform MaskedFillArea;

    public RectTransform BackGround;

    public Color SelectColor;

    public bool IsPlaySound;

    private Slider slider;
    private Color OrgColor;
    private float MaxWidth;
    private MainMenuManager mainMenuManager;
    private void Awake()
    {
    }

    private void Start()
    {
        mainMenuManager=MainMenuManager.GetInstance();
        OrgColor = BackGround.GetComponent<Image>().color;
        slider=GetComponent<Slider>();
        MaskedFillArea.localPosition = new Vector3(-(1 - slider.value) * Mask.rect.width, MaskedFillArea.localPosition.y, MaskedFillArea.localPosition.z);
    }
    public void OnValueChange(float value)
    {
        MaskedFillArea.localPosition = new Vector3(-(1-value)* Mask.rect.width, MaskedFillArea.localPosition.y, MaskedFillArea.localPosition.z);
        if(IsPlaySound)
        {
            mainMenuManager.OnSliderMove();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        mainMenuManager.OnButtonHover();
        BackGround.GetComponent<Image>().color = SelectColor;
    }
    public float X;
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            BackGround.GetComponent<Image>().color = OrgColor;
        }
    }
}
