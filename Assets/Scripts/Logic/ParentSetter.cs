using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentSetter : MonoBehaviour
{
    [Header("�������")]
    public float EnterDistance;
    
    private GameDataCenter dataCenter;
    
    private void Start()
    {
        dataCenter=GameDataCenter.GetInstance();
    }

    private void Update()
    {
        foreach(GameObject gameObject in dataCenter.Planets)
        {
            
            if ((transform.position - gameObject.transform.position).magnitude <= EnterDistance+gameObject.GetComponent<Planet>().GetR())
            {
                transform.parent = gameObject.transform;
                return;
            }
        }
        transform.parent = null;
    }
}
