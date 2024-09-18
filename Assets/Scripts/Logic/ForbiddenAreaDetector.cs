using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenAreaDetector : MonoBehaviour
{
    [Header("�ָ���")]
    public Transform RecoveringPoint;

    private ForbiddenAreaCenter center;
    
    private void Start()
    {
        center=ForbiddenAreaCenter.GetInstance();
    }
    private void Update()
    {
        if (center.CheckPointIsInForbiddenArea(transform.position))
        {
            OnRecovering?.Invoke();
            transform.position = RecoveringPoint.position;
            transform.rotation = RecoveringPoint.rotation;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    /// <summary>
    /// ������������ָ�ʱִ��
    /// </summary>
    public event Action OnRecovering;

}
