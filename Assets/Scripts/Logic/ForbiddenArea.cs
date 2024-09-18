using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenArea : MonoBehaviour
{
    [Header("ÇøÓòÀàĞÍ")]
    public AreaType areaType;
    [Header("°ë¾¶")]
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
