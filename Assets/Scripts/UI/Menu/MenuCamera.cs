using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform RotateCenter;

    public float RotateSpeed;

    private void Update()
    {
        Vector3 Axis = Vector3.Cross(transform.position - RotateCenter.position, RotateCenter.right);
        transform.RotateAround(RotateCenter.position,Axis,RotateSpeed*Time.deltaTime);
        transform.LookAt(RotateCenter.position);
    }
}
