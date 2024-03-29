using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataCenter : MonoBehaviour
{
    #region Singleton

    private static GameDataCenter instance;

    public static GameDataCenter GetInstance()
    {
        return instance;
    }

    #endregion

    [Header("��ǰ���ƶ���")]
    public GameObject Character;
    [Header("������")]
    public Transform AxisPoint;
    private void Awake()
    {
        instance = this;
    }
}
