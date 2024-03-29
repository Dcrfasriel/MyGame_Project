using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEventCenter : MonoBehaviour
{
    [Header("½ÇÉ«")]
    public GameObject Character;

    private BoxCheckPoint CheckPoint;
    private void Awake()
    {
        instance = this;
        CheckPoint = Character.transform.Find("GroundCheckPoint").gameObject.GetComponent<BoxCheckPoint>();
    }

    #region Singleton

    private static CharacterEventCenter instance;

    public static CharacterEventCenter GetInstance()
    {
        return instance;
    }

    
    #endregion

    #region Floating
    private void CheckFloating()
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

    #region OnGround

    public bool IsOnGround
    {
        get { return CheckPoint.IsOnGround; }
    }

    #endregion

    private void Update()
    {
        CheckFloating();
    }
}
