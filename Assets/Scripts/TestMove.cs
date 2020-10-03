using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    
    public int speed;
    public Vector3 direction;
    Rigidbody rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


 
    void FixedUpdate()
    {
        rb.velocity = direction * speed * Time.deltaTime;
    }


}
