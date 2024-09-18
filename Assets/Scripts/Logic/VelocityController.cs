using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class VelocityController : MonoBehaviour
{
    [Header("运动函数")]
    public Func<float, Vector3> GetPosFunc;
    [Header("是否为静止中心")]
    public bool IsOrgCenter;

    private Rigidbody rb;
    private VelocitySystem velocitySystem;
    private Vector3 OrgPos;
    private Vector3 SystemOffset;
    private bool IsMoving=true;
    private Vector3 LastPosition;
    private Vector3 LastVelocity;
    private VelocityObserver Vobserver;

    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
        Vobserver=GetComponent<VelocityObserver>();
        OrgPos = transform.position;
        if (IsOrgCenter)
        {
            GetPosFunc = (t) => OrgPos;
        }
    }

    private void Start()
    {
        velocitySystem = VelocitySystem.GetInstance();
    }

    private void Update()
    {

        if (IsMoving)
        {
            rb.MovePosition(GetPosFunc(velocitySystem.TotalTime) + SystemOffset);
            //transform.position = GetPosFunc(velocitySystem.TotalTime)+SystemOffset;
        }
    }

    public void SetSystemOffset(Vector3 systemOffset)
    {
        this.SystemOffset = systemOffset;
    }

    public void SetCenter(bool TrueOrFalse)
    {
        IsMoving = !TrueOrFalse;
    }

    public Vector3 GetLastVelocity()
    {
        return Vobserver.GetVelocity();
    }


}
