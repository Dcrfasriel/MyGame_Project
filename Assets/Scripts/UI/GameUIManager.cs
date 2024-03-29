using Assets.Frame.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    #region Singleton

    private static GameUIManager uiManeger;

    public static GameUIManager GetInstance() { return uiManeger; }

    #endregion

    [Header("±Ì≤„UI")]
    public SurfaceUI SurfaceUI;
    private void Awake()
    {
        uiManeger = this;
    }
}
