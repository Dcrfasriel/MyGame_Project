using Assets.Frame.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ButtonBase : MonoBehaviour, IPointerEnterHandler,ISelectHandler
{
    public GameObject mainMenuManager;
    private IUIManager uIManager;

    private void Start()
    {
        uIManager=mainMenuManager.GetComponent<IUIManager>();
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        uIManager.OnButtonHover();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(!(eventData is PointerEventData))
        {
            uIManager.OnButtonHover();
        }
    }
}
