using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public BGCurve curve;

    public Transform Head;   
    public Transform Tail;

    public GameObject ModelContent;
    private List<Transform> _parts;

    public float Speed = 10;
    public float Length = 6;

    Vector3 _input;

    void Start()
    {
        
        Length = (Head.position - Tail.position).magnitude;
        var parts = ModelContent.GetComponentsInChildren<Transform>();
        _parts = new List<Transform>(parts.Length);
        foreach (var item in parts)
        {
            if (item != ModelContent.transform)
                _parts.Add(item);
        }
        print("Count = " + _parts.Count);

        print(curve.Points[0].ControlFirstLocal);

        //foreach (var item in collection)
        //{

        //}
                
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            print("asdf");
        }

        /*
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        _input = new Vector3(horizontal, vertical, 0 );

        var nextHeadPosition = (Head.transform.position + _input * Speed * Time.deltaTime);

        var distanceForNextHeadPosition = (Tail.position - nextHeadPosition).magnitude;
        if (distanceForNextHeadPosition < Length)
        {
            Head.transform.position = nextHeadPosition;
        } 
         */
    }


}
