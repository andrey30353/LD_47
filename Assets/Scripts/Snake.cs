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
    BGCurvePointI midPoint;
    BGCurvePointI tailPoint;

    float minDistance;

    Vector3 _input;

    // чем управляем сейчас
    bool useHead;

    // куда смотрим - через координаты
    bool rightView;

    void Start()
    {
        useHead = true;
        rightView = true;

        math = Curve.GetComponent<BGCcMath>();
        headPoint = Curve.Points[0];
        midPoint = Curve.Points[1];
        tailPoint = Curve.Points[2];
        
        Length = math.GetDistance();
        minDistance = Length * 0.5f;

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

        _input = new Vector3(horizontal, vertical, 0);

        //print(headPoint.PositionWorld);
        //print(tailPoint.PositionWorld);
        UpdatePosition();        

        // отменяем все, если длина увеличилась
        var currentLength = (headPoint.PositionWorld - tailPoint.PositionWorld).magnitude;
        if (currentLength > Length || currentLength < minDistance)
        {
            RevertPosition();
        }                    

        UpdateMidPoint();

        UpdateBones();
    }

    private void UpdatePosition()
    {
        if (useHead)
        {
            headPoint.PositionWorld = headPoint.PositionWorld + _input * Speed * Time.deltaTime;
        }
        else
        {
            tailPoint.PositionWorld = tailPoint.PositionWorld + _input * Speed * Time.deltaTime;
        }
    }

    private void RevertPosition()
    {
        if (useHead)
        {
            headPoint.PositionWorld = headPoint.PositionWorld - _input * Speed * Time.deltaTime;
        }
        else
        {
            tailPoint.PositionWorld = tailPoint.PositionWorld - _input * Speed * Time.deltaTime;
        }
    }

    private void UpdateBones()
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

    private void UpdateMidPoint()
    {
        var upVector = Vector3.up;
        if (!rightView)
            upVector = Vector3.down;

        var middle = (headPoint.PositionWorld + tailPoint.PositionWorld) * 0.5f;// Vector3.Project(headPoint.PositionWorld, tailPoint.PositionWorld);
        var normal = Vector3.Cross(headPoint.PositionWorld, tailPoint.PositionWorld);

        var proj = GetProjected(headPoint.PositionWorld, tailPoint.PositionWorld, middle);
        //var pp = Vector3.Project(c - s, Vector3.up);
        
        var currentLength = (headPoint.PositionWorld - tailPoint.PositionWorld).magnitude;
        var freeLength = Mathf.Clamp(Length - currentLength, 0, Length);

        midPoint.PositionWorld = middle + Vector3.up * freeLength;

        // todo
        //Debug.DrawLine(normal, normal * upVector);
    }


    private void OnDrawGizmos()
    {

        var upVector = Vector2.up;
        if (!rightView)
            upVector = Vector2.down;

        var currentLength = (headPoint.PositionWorld - tailPoint.PositionWorld).magnitude;
        var freeLength = Mathf.Clamp(Length - currentLength, 0, Length);

        var middle = (headPoint.PositionWorld + tailPoint.PositionWorld) * 0.5f;// Vector3.Project(headPoint.PositionWorld, tailPoint.PositionWorld);
         var normal = Vector3.Cross(headPoint.PositionWorld, tailPoint.PositionWorld);

        var proj = GetProjected(headPoint.PositionWorld, tailPoint.PositionWorld, middle + Vector3.up * freeLength);

        var difY = headPoint.PositionWorld.y - tailPoint.PositionWorld.y;
               
        var cc = Vector3.Reflect(middle, Vector3.down);
        //Vector3 heading = target.position - transform.position;
        //Vector3 force = Vector3.Project(heading, railDirection);

        //print($"Length = {Length}; currentLength = {currentLength}");
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(middle, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(middle + Vector3.up * freeLength + difY * Vector3.left, 0.1f);        
        

        /*
        //====Position and Tangent (World)       
        //get position and tangent at the center of the spline
        Vector3 tangAtSplineCenter;    

        for (int i = 0; i < _parts.Count; i++)
        {
            var posAtSplineCenter = math.CalcPositionAndTangentByDistanceRatio((float)i / _parts.Count, out tangAtSplineCenter);
            Gizmos.DrawCube(posAtSplineCenter, new Vector3(0.1f, 0.1f, 0.1f));
        }*/
    }
    private Vector3 GetProjected(Vector3 s, Vector3 f, Vector3 c)
    {
        Vector3 startToFinish = f - s;
        Vector3 prj = Vector3.Project(c - s, startToFinish);
        return prj + s;
    }

   
   
}
