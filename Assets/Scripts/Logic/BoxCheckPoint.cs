using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCheckPoint : MonoBehaviour
{
    [Header("Вуекеж")]
    public LayerMask layerMask;
    public bool IsOnGround { get;private set; }

    public event Action<Collider> OnEnter;
    public event Action<Collider> OnStay;
    public event Action<Collider> OnExit;
    private void OnTriggerEnter(Collider other)
    {
        if((layerMask.value & (2<< (other.gameObject.layer - 1))) == 0)return;
        OnEnter?.Invoke(other);
        IsOnGround = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((layerMask.value& (2 << (other.gameObject.layer-1))) ==0) return;
        OnStay?.Invoke(other);
        IsOnGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if ((layerMask.value & (2 << (other.gameObject.layer - 1))) == 0) return;
        OnExit?.Invoke(other);
        IsOnGround = false;
    }

    private void OnEnable()
    {
        IsOnGround = false;
    }
}
