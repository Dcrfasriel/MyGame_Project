using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisPointFol : MonoBehaviour
{
    [Header("�������")]
    public Transform Follow;
    [Header("��ʼ��������")]
    public Vector3 LocalPosition;

    private void Update()
    {
        transform.position =Follow.position+Follow.up*LocalPosition.y+Follow.right*LocalPosition.x+Follow.forward*LocalPosition.z;
    }
}
