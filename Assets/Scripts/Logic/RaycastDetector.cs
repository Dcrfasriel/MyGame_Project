using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    [Header("����ƫ����")]
    public Vector3 Offset;
    [Header("��С�ٶ�")]
    public float MinSpeed;
    [Header("������С�ٶ�")]
    public float HLevelMinSpeed;
    [Header("���ߵĳ���")]
    public float rayLength = 10f; // ���ߵĳ���
    [Header("�������ߵĳ���")]
    public float HLevelrayLength = 40f; // ���ߵĳ���
    [Header("������")]
    public LayerMask collisionLayer; // �����߼���������ض�����

    private Rigidbody rb; // ��Ϸ����ĸ������

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // ��ȡ��Ϸ�����ϵĸ������
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
        Vector3 direction = rb.velocity.normalized; // ��ȡ��Ϸ������˶�����

        // ����Ϸ����ǰ���������ߣ������˶�����
        bool isHit = Physics.Raycast(transform.position+Offset, direction, out hit, rayLength, collisionLayer);

        if(isHit)
        {
            rb.velocity = Vector3.zero; // �����ٶ�Ϊ�㣬ֹͣ�˶�
            rb.angularVelocity = Vector3.zero; // ֹͣ��ת
            rb.position = hit.point-Offset;
        }

        return;
    }

    private void HlevelCheckCollisionAhead()
    {
        RaycastHit hit;
        Vector3 direction = rb.velocity.normalized; // ��ȡ��Ϸ������˶�����

        // ����Ϸ����ǰ���������ߣ������˶�����
        bool isHit = Physics.Raycast(transform.position, direction, out hit, HLevelrayLength, collisionLayer);

        if (isHit)
        {
            rb.velocity = Vector3.zero; // �����ٶ�Ϊ�㣬ֹͣ�˶�
            rb.angularVelocity = Vector3.zero; // ֹͣ��ת
            rb.position = hit.point;
        }

        return;
    }
}
