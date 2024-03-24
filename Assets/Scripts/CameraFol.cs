using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFol : MonoBehaviour
{
    [Header("中心游戏对象")]
    public Transform Center;
    [Header("灵敏度")]
    public float Speed;
    [Header("抬高限制")]
    public float UpLimit;
    [Header("降低限制")]
    public float DownLimit;
    [Header("是否锁定鼠标")]
    public bool IsLock;
    [Header("最大摄像机距离(-1无限制)")]
    public float MaxDistance;
    [Header("检测层遮罩")]
    public int RayLayerMask;

    private Camera Mcamera;
    private CharacterEventCenter characterEventCenter;
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
    }
    private void LateUpdate()
    {
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
        float h = Input.GetAxis("Mouse X"), v = Input.GetAxis("Mouse Y");
        transform.RotateAround(Center.position, Center.up, h * Speed);
        Vector3 Axis = Vector3.Cross(Center.up, transform.position - Center.position);
        transform.RotateAround(Center.position, Axis, v * Speed);
        float angle = 90 - Vector3.Angle(transform.position - Center.position, Center.up);
        if (angle < DownLimit)
        {
            transform.RotateAround(Center.position, Axis, -v * Speed);
        }
        else if (angle > UpLimit)
        {
            transform.RotateAround(Center.position, Axis, -v * Speed);
        }
        transform.LookAt(Center.position, Center.up);
    }
    private void Floating()
    {
        float h = Input.GetAxis("Mouse X"), v = Input.GetAxis("Mouse Y");
        Vector3 HorWorld = Center.right;
        Vector3 VerWorld = Center.up;
        Center.RotateAround(Center.position,VerWorld, h * Speed);
        Center.RotateAround(Center.position,HorWorld, -v * Speed);
        transform.position=Center.position+Center.forward*-MaxDistance;
        transform.LookAt(Center.position,Center.up);
    }

    private void AdjustDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(Center.position, transform.position - Center.position, out hit, MaxDistance, RayLayerMask))
        {
            transform.position = transform.position+ (hit.point-transform.position)* 0.8f;
        }
        else
        {
            transform.localPosition = Vector3.Normalize(transform.localPosition) * MaxDistance;
        }
    }
}
