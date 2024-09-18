using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisPointFol : MonoBehaviour
{
    [Header("�������")]
    public Transform Follow;
    [Header("��ʼ��������")]
    public Vector3 LocalPosition;
    [Header("�Ƿ�������ת")]
    public bool IsRotationLock;

    private Transform OrgFollow;
    private Vector3 OrgLocalPosition;
    private bool OrgIsRotationLock;

    private void Start()
    {
        OrgFollow = Follow;
        OrgLocalPosition = LocalPosition;
        OrgIsRotationLock = IsRotationLock;
    }
    private void Update()
    {
        transform.position =Follow.position+Follow.up*LocalPosition.y+Follow.right*LocalPosition.x+Follow.forward*LocalPosition.z;
        if (IsRotationLock)
        {
            transform .rotation = Follow.rotation;
        }
    }

    public void SetFollow(Transform Follow,Vector3 LocalPos,bool IsRotationLock=false)
    {
        this.Follow = Follow;
        this.LocalPosition = LocalPos;
        this.IsRotationLock= IsRotationLock;
    }

    public void RecoverData()
    {
        this.Follow = OrgFollow;
        this.LocalPosition = OrgLocalPosition;
        this.IsRotationLock = OrgIsRotationLock;
    }
}
