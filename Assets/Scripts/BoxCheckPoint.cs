using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCheckPoint : MonoBehaviour
{
    public bool IsOnGround { get;private set; }

    public event Action<Collider> OnEnter;
    public event Action<Collider> OnStay;
    public event Action<Collider> OnExit;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer!=6)return;
        OnEnter?.Invoke(other);
        IsOnGround = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 6) return;
        OnStay?.Invoke(other);
        IsOnGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 6) return;
        OnExit?.Invoke(other);
        IsOnGround = false;
    }
}
