using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCenter : MonoBehaviour
{
    #region Singleton

    private static DebugCenter instance;

    public static DebugCenter GetInstance()
    {
        return instance;
    }

    #endregion
    
    public bool IsDebugUIActive;
    private void Awake()
    {
        instance = this;
    }
}
