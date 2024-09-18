using Assets.Frame.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Planet : MonoBehaviour
{
    [Header("��������")]
    public float PG;
    [Header("��ɫ�������")]
    public float EnterDistance;
    [Header("�������л�Ϊ�˶���ľ���")]
    public float CenterEnterDistance;
    [Header("��ת�ٶ�")]
    public float SelfRotationSpeed;
    [Header("��ת��")]
    public Vector3 SelfRotationAxis;
    [Header("��ת����")]
    public Transform RotationPoint;
    //[Header("����������")]
    //public float CenterPM;
    [Header("��ת�ٶ�")]
    public float RotateSpeed;
    [Header("��ת��")]
    public Vector3 RotationAxis;
    [Header("�Ƿ����ö�����ת")]
    public bool ActivateChildCenter;
    [Header("������ת����")]
    public Planet ChildCenter;
    [Header("������ת��")]
    public Vector3 ChildAxis;
    [Header("������ת�ٶ�")]
    public float ChildSpeed;
    [Header("�Ƿ�Ϊ��̬����")]
    public bool IsStatic;


    private CharacterEventCenter characterEventCenter;
    private GameDataCenter gameDataCenter;
    private float R;
    private VelocityController velocityController;
    private ForceSystem forceSystem;
    private Vector3 OrgPosition;
    private Vector3 OrgRotationPoint;
    private Vector3 ChildOrg;

    private void Awake()
    {
        R = transform.localScale.y;
        OrgPosition = transform.position;
        velocityController = GetComponent<VelocityController>();
        Vector3 Line = RotationPoint.position + RotationAxis;
        OrgRotationPoint = RotationPoint.position;
        Vector3 ChildOrg = Vector3.zero;
        if (ChildCenter != null )
        {
            ChildOrg = ChildCenter.transform.position;
        }
        if (IsStatic)
        {
            velocityController.GetPosFunc = (t) =>
            {
                return OrgPosition;
            };
        }
        else
        {
            velocityController.GetPosFunc = (t) =>
            {
                if(!ActivateChildCenter)
                {
                    return Tools.GetPosition(OrgPosition, OrgRotationPoint, RotationAxis, CalcualateSpeed(), t);
                }
                else
                {

                    return ChildCenter.GetPos(t)- ChildOrg+
                    Tools.GetPosition(OrgPosition,ChildOrg, ChildAxis, ChildSpeed, t);
                }
            };
        }
    }
    public Vector3 GetPos(float t)
    {
        if (!ActivateChildCenter)
        {
            return Tools.GetPosition(OrgPosition, OrgRotationPoint, RotationAxis, CalcualateSpeed(), t);
        }
        else
        {

            return ChildCenter.GetPos(t) - ChildOrg +
            Tools.GetPosition(OrgPosition, ChildOrg, ChildAxis, ChildSpeed, t);
        }
    }
    private void Start()
    {
        characterEventCenter=CharacterEventCenter.GetInstance();
        forceSystem = ForceSystem.GetInstance();
        gameDataCenter=GameDataCenter.GetInstance();
        }

    private void Update()
    {
        forceSystem.SetGravityForce(gameObject.name + "Gravity", PG, transform.position);
        SetCharacterDirection();
        transform.Rotate(SelfRotationAxis, SelfRotationSpeed * Time.deltaTime);
    }
    private float CalcualateSpeed()
    {
        //if (RotationPoint == null)
        //{
        //    return 0;
        //}
        //float r = (transform.position - RotationPoint.position).magnitude;
        //return Mathf.Pow(CenterPM / r, 0.5f);
        return RotateSpeed;
    }

    private void SetCharacterDirection()
    {
        if ((gameDataCenter.AxisPoint.position - transform.position).magnitude <= CenterEnterDistance + R)
        {
            VelocitySystem.GetInstance().SetCenter(velocityController);
        }
        if ((gameDataCenter.AxisPoint.transform.position - transform.position).magnitude <= EnterDistance + R)
        {
            characterEventCenter.SetCurrentCenter(transform.position);
        }
    }


    public float GetR()
    {
        return R;
    }
}
