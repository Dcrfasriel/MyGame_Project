using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ProcessIndicatorController : MonoBehaviour
{
    [Header("平滑移动速度")]
    public float SmoothSpeed;

    private Slider slider;
    private Text indText;
    private float ActualValue;
    private float IndicateValue;
    private void Awake()
    {
        slider=GetComponent<Slider>();
        indText=transform.Find("PercentText").GetComponent<Text>();
    }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetActualValue(float process)
    {
        IndicateValue = process;
        ActualValue = process;
    }

    public void SetSmoothValue(float process)
    {
        ActualValue=process;
    }

    private void Update()
    {
        IndicateValue=NumberMoveTowards(IndicateValue, ActualValue,SmoothSpeed);
        slider.value = IndicateValue;
        indText.text = (IndicateValue * 100).ToString("0.00") + "%";
    }

    private float NumberMoveTowards(float number, float target, float speed)
    {
        // 计算数值朝向目标变化的方向
        float direction = Mathf.Sign(target - number);

        // 根据速度和时间间隔计算每次变化的距离
        float step = speed * Time.deltaTime;

        // 计算新的数值，确保不会超过目标值
        float newNumber = number + direction * step;
        if (direction > 0)
        {
            newNumber = Mathf.Min(newNumber, target);
        }
        else
        {
            newNumber = Mathf.Max(newNumber, target);
        }

        return newNumber;
    }
}
