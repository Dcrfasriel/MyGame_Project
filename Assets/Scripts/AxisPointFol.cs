using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisPointFol : MonoBehaviour
{
    [Header("跟随对象")]
    public Transform Follow;
    [Header("初始本地坐标")]
    public Vector3 LocalPosition;

    private void Update()
    {
        transform.position =Follow.position+Follow.up*LocalPosition.y+Follow.right*LocalPosition.x+Follow.forward*LocalPosition.z;
    }
}
