using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuger : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.velocity = Vector3.up;
    }
    
}
