using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ForceSystem : MonoBehaviour
{
    #region Singleton

    private static ForceSystem instance;

    public static ForceSystem GetInstance()
    {
        return instance;
    }

    #endregion

    [Header("控制对象")]
    public List<GameObject> ControllingObjects;

    public Dictionary<string, (float Force,Vector3 Position)> GravityForceTable=new Dictionary<string, (float, Vector3)>();
    public Dictionary<string,(float R, Vector3 Position)> ExtremForceTable = new Dictionary<string, (float, Vector3)>();

    private List<Rigidbody> Rigibodies;
    private List<IRecieveForce> FuncClasses;
    private void Awake()
    {
        instance = this;
        Rigibodies = ControllingObjects.Select(p => p.GetComponent<Rigidbody>()).ToList();
        FuncClasses = ControllingObjects.Select(p =>
        {
            IRecieveForce recieveForce=p.GetComponent<IRecieveForce>();
            if (recieveForce == null) Debug.Log("未设置受力组件");
            return recieveForce;
        }).ToList();
    }
    public void SetGravityForce(string Name,float Force,Vector3 Position)
    {
        GravityForceTable[Name]= (Force,Position);
    }
    public void SetExtremForce(string Name, float R, Vector3 Position)
    {
        ExtremForceTable[Name] = (R, Position);
    }

    private void Update()
    {
        for(int i=0;i<ControllingObjects.Count;i++)
        {
            Rigidbody rb = Rigibodies[i];
            IRecieveForce recieveForce = FuncClasses[i];
            Vector3 Gravity = CalcualateGravityAcceleration(ControllingObjects[i].transform.position + rb.centerOfMass);
            recieveForce.SetForce(Gravity);
        }
    }

    public Vector3 CalcualateGravityAcceleration(Vector3 Point,string Name="")
    {
        Vector3 Force= Vector3.zero;
        foreach (var key in GravityForceTable.Keys)
        {
            if (string.Compare(key, Name) == 0) continue;
            var value = GravityForceTable[key];
            Force += Vector3.Normalize(value.Position- Point) *value.Force / (Point - value.Position).sqrMagnitude;
        }

        return Force;
    }
}
