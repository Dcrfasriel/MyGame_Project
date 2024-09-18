using Assets.Frame.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Planet : MonoBehaviour
{
    [Header("引力参数")]
    public float PG;
    [Header("角色进入距离")]
    public float EnterDistance;
    [Header("将中心切换为此对象的距离")]
    public float CenterEnterDistance;
    [Header("自转速度")]
    public float SelfRotationSpeed;
    [Header("自转轴")]
    public Vector3 SelfRotationAxis;
    [Header("公转中心")]
    public Transform RotationPoint;
    //[Header("中心力参数")]
    //public float CenterPM;
    [Header("公转速度")]
    public float RotateSpeed;
    [Header("公转轴")]
    public Vector3 RotationAxis;
    [Header("是否启用二级公转")]
    public bool ActivateChildCenter;
    [Header("二级公转中心")]
    public Planet ChildCenter;
    [Header("二级公转轴")]
    public Vector3 ChildAxis;
    [Header("二级公转速度")]
    public float ChildSpeed;
    [Header("是否为静态对象")]
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
