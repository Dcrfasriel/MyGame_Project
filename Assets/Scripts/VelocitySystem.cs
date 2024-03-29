using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VelocitySystem : MonoBehaviour
{
    #region Singleton

    private static VelocitySystem instance;

    public static VelocitySystem GetInstance()
    {
        return instance;
    }

    #endregion

    [Header("¾²Ö¹ÖÐÐÄ")]
    public VelocityController OrgCenter;

    //public VelocityController Test;
    private List<GameObject> ControllingObjects;
    private CharacterEventCenter eventCenter;
    public VelocityController Center;
    private GameDataCenter gameDataCenter;
    private List<VelocityController> velocitySystems;
    private Vector3 SystemOffset=Vector3.zero;
    public float TotalTime = 0.0f;

    private void Awake()
    {
        instance = this;
        ControllingObjects = new List<GameObject>();
        ControllingObjects.AddRange(GameObject.FindGameObjectsWithTag("Planet"));
        velocitySystems = ControllingObjects.Select((a) => a.GetComponent<VelocityController>()).ToList();
        Center = OrgCenter;
    }
    private void Start()
    {
        eventCenter=CharacterEventCenter.GetInstance();
        gameDataCenter=GameDataCenter.GetInstance();
    }
    public Vector3 GetSystemOffsetFunc()
    {
        return SystemOffset;
    }
    private void Update()
    {
        if (eventCenter.IsFloating)
        {
            SetCenter(OrgCenter);
        }
        TotalTime += Time.deltaTime;
        SystemOffset =Center.transform.position- Center.GetPosFunc(TotalTime);
        foreach (var v in velocitySystems)
        {
            v.SetSystemOffset(SystemOffset);
        }
    }

    public void SetCenter(VelocityController Newcenter)
    {
            if (Newcenter == null) Newcenter = OrgCenter;
        if (Center == Newcenter) return;
        Center.SetCenter(false);
        Center = Newcenter;
        gameDataCenter.Character.GetComponent<Rigidbody>().velocity = gameDataCenter.Character.GetComponent<CharacterMove>().GetLastSpeed()-Center.GetLastVelocity();
        Center.SetCenter(true);
    }

    
}
