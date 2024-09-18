using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReciever : MonoBehaviour, IRecieveForce
{
    Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    void IRecieveForce.SetForce(Vector3 Gravity)
    {
        body.AddForce(Gravity,ForceMode.Acceleration);
    }
}
