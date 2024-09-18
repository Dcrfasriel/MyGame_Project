using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ProcessIndicatorController : MonoBehaviour
{
    [Header("ƽ���ƶ��ٶ�")]
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
        // ������ֵ����Ŀ��仯�ķ���
        float direction = Mathf.Sign(target - number);

        // �����ٶȺ�ʱ��������ÿ�α仯�ľ���
        float step = speed * Time.deltaTime;

        // �����µ���ֵ��ȷ�����ᳬ��Ŀ��ֵ
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
