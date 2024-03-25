using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEventCenter : MonoBehaviour
{
    #region Singleton

    private static CharacterEventCenter instance;

    public static CharacterEventCenter GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region Floating

    private int FloatingCount = 0;
    public bool IsFloating
    {
        get { return FloatingCount >= 5; }
    }
    public Vector3? CurrentCenter=null;
    public void SetCurrentCenter(Vector3 Center)
    {
        FloatingCount = 0;
        if(CurrentCenter!=Center)
        {
            OnCenterChange?.Invoke(Center);
        }
        CurrentCenter = Center;

    }

    public event Action<Vector3?> OnCenterChange;

    #endregion

    private void Update()
    {
        FloatingCount++;
        if (FloatingCount >= 5)
        {
            FloatingCount = 5;
            if (CurrentCenter != null)
            {
                OnCenterChange?.Invoke(null);
            }
            CurrentCenter = null;
        }
    }
}
