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

    public List<GameObject> ControllingObjects;

    //public VelocityController Test;

    public VelocityController Center;
    private GameDataCenter gameDataCenter;
    private List<VelocityController> velocitySystems;
    private Vector3 SystemOffset=Vector3.zero;
    public float TotalTime = 0.0f;

    private void Awake()
    {
        instance = this;
        velocitySystems = ControllingObjects.Select((a) => a.GetComponent<VelocityController>()).ToList();
        Center = OrgCenter;
    }
    private void Start()
    {
        gameDataCenter=GameDataCenter.GetInstance();
    }
    public Vector3 GetSystemOffsetFunc()
    {
        return SystemOffset;
    }
    private void Update()
    {
        TotalTime += Time.deltaTime;
        //if(Center!=Test)SetCenter(Test);
        if(Center!=null)SystemOffset =Center.transform.position- Center.GetPosFunc(TotalTime);
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
