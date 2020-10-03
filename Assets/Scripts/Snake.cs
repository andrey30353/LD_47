using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public BGCurve Curve;
    public BGCcMath math;   

    public Transform Head;    
    public Transform Tail;     

    public float Speed = 10;
    public float Length = 6;

    public GameObject ModelContent;
    public List<Transform> _parts;

    BGCurvePointI headPoint;
    BGCurvePointI tailPoint;

    Vector3 _input;

    bool useHead;

    void Start()
    {
        useHead = true;

        math = Curve.GetComponent<BGCcMath>();
        headPoint = Curve.Points[0];
        tailPoint = Curve.Points[Curve.Points.Length - 1];
        
        //Length = (Head.position - Tail.position).magnitude;

        //var parts = ModelContent.GetComponentsInChildren<Transform>();
        //_parts = new List<Transform>(parts.Length);
        //foreach (var item in parts)
        //{
        //    if (item != ModelContent.transform)
        //        _parts.Add(item);
        //}
        print("Count = " + _parts.Count);

        // print(curve.Points[0].ControlFirstLocal);

        //foreach (var item in collection)
        //{

        //}

        //SetPosition();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            useHead = !useHead;
        }

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        _input = new Vector3(horizontal, vertical, 0 );

        //print(headPoint.PositionWorld);
        //print(tailPoint.PositionWorld);
        if(useHead)
        {
            headPoint.PositionWorld = headPoint.PositionWorld + _input * Speed * Time.deltaTime;
        }            
        else
        {
            tailPoint.PositionWorld = tailPoint.PositionWorld + _input * Speed * Time.deltaTime;
        }

        /*
                var nextHeadPosition = (headPoint.PositionWorld + _input * Speed * Time.deltaTime);

                var distanceForNextHeadPosition = (tailPoint.PositionWorld - nextHeadPosition).magnitude;
                if (distanceForNextHeadPosition < Length)
                {
                    Head.transform.position = nextHeadPosition;
                }       
                */
        SetPosition();

    }

    private void SetPosition()
    {       
        //get position at the center of the spline  
        Vector3 tangAtSplineCenter;

        _parts[0].position = math.CalcPositionAndTangentByDistanceRatio(0f, out tangAtSplineCenter);
        _parts[0].rotation = Quaternion.LookRotation(tangAtSplineCenter) * Quaternion.Euler(0, 90, 0);
        for (int i = 1; i < 10; i++)
        {
            var posAtSplineCenter = math.CalcPositionAndTangentByDistanceRatio(i * 0.1f, out tangAtSplineCenter);
            _parts[i].position = posAtSplineCenter;
            _parts[i].rotation = Quaternion.LookRotation(tangAtSplineCenter) * Quaternion.Euler(0, 90, 0);
        }
    }

    private void OnDrawGizmos()
    {
        //====Position and Tangent (World)       
        //get position and tangent at the center of the spline
        Vector3 tangAtSplineCenter;
     

        for (int i = 0; i < _parts.Count; i++)
        {
            var posAtSplineCenter = math.CalcPositionAndTangentByDistanceRatio((float)i / _parts.Count, out tangAtSplineCenter);
            Gizmos.DrawCube(posAtSplineCenter, new Vector3(0.1f, 0.1f, 0.1f)/*0.05f*/);
        }
    }


}
