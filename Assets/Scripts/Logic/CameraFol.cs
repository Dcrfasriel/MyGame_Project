using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFol : MonoBehaviour
{
    [Header("������Ϸ����")]
    public Transform Center;
    [Header("������")]
    public float Speed;
    [Header("̧������")]
    public float UpLimit;
    [Header("��������")]
    public float DownLimit;
    [Header("�Ƿ��������")]
    public bool IsLock;
    [Header("������������(-1������)")]
    public float MaxDistance;
    [Header("��������")]
    public LayerMask RayLayerMask;
    [Header("�Ƿ�������������Transform��ͬ")]
    public bool IsLockTransform;

    private Camera Mcamera;
    private CharacterEventCenter characterEventCenter;
    private float XInput;
    private float YInput;
    private void Awake()
    {
        Mcamera = GetComponent<Camera>();
        transform.parent = Center;
    }
    private void Start()
    {
        characterEventCenter=CharacterEventCenter.GetInstance();
        if (IsLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        UpLimit = Mathf.Clamp(UpLimit, -90, 90);
        DownLimit = Mathf.Clamp(DownLimit, -90, UpLimit);
        if (MaxDistance == -1)
        {
            MaxDistance = transform.localPosition.magnitude;
        }
        AdjustDistance();
        GameUIManager.GetInstance().AddEventToSurfaceUI((keys, u) =>
        {
            if (!u)
            {
                XInput = 0; YInput = 0;
                return;
            }
            XInput = Input.GetAxis(keys[0]);
            YInput = Input.GetAxis(keys[1]);
        }, "Mouse X", "Mouse Y");
    }
    private void LateUpdate()
    {
        if (IsLockTransform)
        {
            transform.position=Center.position; transform.rotation=Center.rotation;
            return;
        }
        if(characterEventCenter.IsFloating)
        {
            Floating();
        }
        else
        {
            NonFloating();
        }
        AdjustDistance();
    }

    private void NonFloating()
    {
        transform.RotateAround(Center.position, Center.up, XInput * Speed);
        Vector3 Axis = Vector3.Cross(Center.up, transform.position - Center.position);
        transform.RotateAround(Center.position, Axis, YInput * Speed);
        float angle = 90 - Vector3.Angle(transform.position - Center.position, Center.up);
        if (angle < DownLimit)
        {
            transform.RotateAround(Center.position, Axis, -YInput * Speed);
        }
        else if (angle > UpLimit)
        {
            transform.RotateAround(Center.position, Axis, -YInput * Speed);
        }
        transform.LookAt(Center.position, Center.up);
    }
    private void Floating()
    {
        Vector3 HorWorld = Center.right;
        Vector3 VerWorld = Center.up;
        Center.RotateAround(Center.position,VerWorld, XInput * Speed);
        Center.RotateAround(Center.position,HorWorld, -YInput * Speed);
        transform.position=Center.position+Center.forward*-MaxDistance;
        transform.LookAt(Center.position,Center.up);
    }

    private void AdjustDistance()
    {
        RaycastHit hit;
        if((transform.position - Center.position).sqrMagnitude <= 0.01)
        {
            transform.localPosition = Vector3.back;
        }
        if (Physics.Raycast(Center.position, transform.position - Center.position, out hit, MaxDistance, RayLayerMask.value))
        {
            transform.position = Center.position + (hit.point- Center.position)*0.8f;
        }
        else
        {
            transform.localPosition = Vector3.Normalize(transform.localPosition) * MaxDistance;
        }
    }
}
