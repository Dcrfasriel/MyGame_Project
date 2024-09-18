using Assets.Frame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackMaskController : MonoBehaviour
{
    [Header("遮罩值")]
    public float MaskValue;
    [Header("变化速度")]
    public float ChangeSpeed;

    private Material MaskMat;
    private bool IsTowardsAppear;
    private bool IsTowardsDisAppear;
    private Image UIImage;

    private void Awake()
    {
        UIImage = GetComponent<Image>();
        MaskMat =UIImage.material;
        MaskMat.SetFloat("_Mask", MaskValue);
        IsTowardsDisAppear = true;
    }
    private void Start()
    {

    }
    private void OnGUI()
    {
        MaskMat.SetFloat("_Mask", MaskValue);
        if(IsTowardsAppear&&IsTowardsDisAppear)
        {
            Debug.LogWarning("Can't Appear and Disappear at same time, force setting disappear false");
            IsTowardsDisAppear = false;
        }
        if(IsTowardsAppear&& MaskValue <= 2)
        {
            MaskValue += ChangeSpeed * Time.deltaTime;
        }
        if(IsTowardsDisAppear && MaskValue >= 0)
        {
            MaskValue -= ChangeSpeed * Time.deltaTime;
        }
        if (MaskValue < 0.1) UIImage.raycastTarget = false;
        else UIImage.raycastTarget = true;
    }
    private void Update()
    {
        OnMaskAppearEvent.CheckUpdate();
        OnMaskDisappearEvent.CheckUpdate();
    }

    private TemporaryEvent OnMaskAppearEvent=new TemporaryEvent();
    private TemporaryEvent OnMaskDisappearEvent=new TemporaryEvent();

    public void SetTemporaryEvent(Action action,bool IsAppear)
    {
        if (IsAppear)
        {
            OnMaskAppearEvent.StartEvent(action, () => MaskValue >= 2);
        }
        else
        {
            OnMaskDisappearEvent.StartEvent(action, () => MaskValue <=0);
        }
    }
    public void SetAppear(bool isAppear)
    {
        IsTowardsAppear = isAppear;
        IsTowardsDisAppear=!isAppear;
    }
}
