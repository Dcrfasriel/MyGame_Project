using Assets.Frame.AttributeSet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BindTransform]
public class MovingPlatform : MonoBehaviour
{
    public List<Vector3> MovingPoint;

    public float Speed;

    private int PointCount=0;
    private void Update()
    {
        if ((transform.position - MovingPoint[PointCount]).sqrMagnitude < 0.5f)
        {
            if(PointCount==MovingPoint.Count-1)
            {
                PointCount = 0;
            }
            else
            {
                PointCount++;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, MovingPoint[PointCount], Speed * Time.deltaTime);
        }
    }
}
