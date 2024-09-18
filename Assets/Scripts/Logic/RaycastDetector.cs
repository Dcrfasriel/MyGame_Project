using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    [Header("射线偏移量")]
    public Vector3 Offset;
    [Header("最小速度")]
    public float MinSpeed;
    [Header("二级最小速度")]
    public float HLevelMinSpeed;
    [Header("射线的长度")]
    public float rayLength = 10f; // 射线的长度
    [Header("二级射线的长度")]
    public float HLevelrayLength = 40f; // 射线的长度
    [Header("层遮罩")]
    public LayerMask collisionLayer; // 将射线检测限制在特定层上

    private Rigidbody rb; // 游戏对象的刚体组件

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // 获取游戏对象上的刚体组件
    }

    void Update()
    {
        if (rb.velocity.magnitude > HLevelMinSpeed)
        {
            HlevelCheckCollisionAhead();
        }
        else if(rb.velocity.magnitude > MinSpeed)
        {
            CheckCollisionAhead();
        } 
    }

    private void CheckCollisionAhead()
    {
        RaycastHit hit;
        Vector3 direction = rb.velocity.normalized; // 获取游戏对象的运动方向

        // 在游戏对象前方发射射线（沿着运动方向）
        bool isHit = Physics.Raycast(transform.position+Offset, direction, out hit, rayLength, collisionLayer);

        if(isHit)
        {
            rb.velocity = Vector3.zero; // 设置速度为零，停止运动
            rb.angularVelocity = Vector3.zero; // 停止旋转
            rb.position = hit.point-Offset;
        }

        return;
    }

    private void HlevelCheckCollisionAhead()
    {
        RaycastHit hit;
        Vector3 direction = rb.velocity.normalized; // 获取游戏对象的运动方向

        // 在游戏对象前方发射射线（沿着运动方向）
        bool isHit = Physics.Raycast(transform.position, direction, out hit, HLevelrayLength, collisionLayer);

        if (isHit)
        {
            rb.velocity = Vector3.zero; // 设置速度为零，停止运动
            rb.angularVelocity = Vector3.zero; // 停止旋转
            rb.position = hit.point;
        }

        return;
    }
}
