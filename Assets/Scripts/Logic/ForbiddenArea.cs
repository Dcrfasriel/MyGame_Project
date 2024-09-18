using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenArea : MonoBehaviour
{
    [Header("��������")]
    public AreaType areaType;
    [Header("�뾶")]
    public float R;
    private ForbiddenAreaCenter center;

    private void Start()
    {
        center = ForbiddenAreaCenter.GetInstance();
        if (areaType == AreaType.Sphere)
        {
            center.AddSphere(transform, R);
        }
    }

    public enum AreaType
    {
        Sphere,
    }
}
