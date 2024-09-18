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

    [Header("当前控制对象")]
    public GameObject Character;
    [Header("相机轴点")]
    public Transform AxisPoint;


    public List<GameObject> Planets = new List<GameObject>();
    private void Awake()
    {
        instance = this;
        Planets.AddRange(GameObject.FindGameObjectsWithTag("Planet"));
    }
}
