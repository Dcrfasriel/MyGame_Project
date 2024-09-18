using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenAreaCenter : MonoBehaviour
{
    #region Singleton

    private static ForbiddenAreaCenter instance;

    public static ForbiddenAreaCenter GetInstance() { return instance; }

    #endregion

    

    private void Awake()
    {
        instance = this;
    }

    private List<(Transform center, float R)> SphereArea = new();

    public void AddSphere(Transform center, float R)
    {
        SphereArea.Add((center, R));
    }

    public bool CheckPointIsInForbiddenArea(Vector3 point)
    {
        foreach(var c in SphereArea)
        {
            if ((point - c.center.position).magnitude < c.R)
            {
                return true;
            }
        }
        return false;
    }

}
