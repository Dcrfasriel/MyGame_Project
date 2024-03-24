using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VelocityObserver : MonoBehaviour
{
    public int WindowLength;

    Vector3[] PosQueue;
    float[] TimeQueue;
    private int Cp = 0;

    private void Awake()
    {
        PosQueue = new Vector3[WindowLength];
        TimeQueue = new float[WindowLength];
        for (int i = 0; i < WindowLength; i++)
        {
            PosQueue[i] = transform.position;
            TimeQueue[i] = Time.deltaTime;
        }
    }
    private void Update()
    {
        Cp++;
        if (Cp >= WindowLength) Cp = 0;
        PosQueue[Cp] = transform.position;
        TimeQueue[Cp] = Time.deltaTime;
    }

    private Vector3 Calcualate()
    {
        Vector3 a = PosQueue[Cp];
        Vector3 b = PosQueue[(Cp + 1 >= WindowLength) ? 0 : Cp + 1];
        return (a - b) / Sum(TimeQueue);
    }

    private float Sum(float[] array)
    {
        float sum = 0;
        foreach (var item in array)
        {
            sum += item;
        }
        return sum;
    }

    public Vector3 GetVelocity()
    {
        return Calcualate();
    }
}
