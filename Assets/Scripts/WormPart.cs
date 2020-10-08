using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WormPartType
{
    Head,
    Mid, 
    Tail
}

public class WormPart : MonoBehaviour
{    
    public Worm worm;

    public WormPartType Type;

    public Collider _otherCollider;

    private void OnTriggerEnter(Collider other)
    {
        _otherCollider = other;
       // print("OnTriggerEnter " + other.name);
        SetInfo(Type);
    }

    private void OnTriggerStay(Collider other)
    {
        _otherCollider = other;
        //print("OnTriggerEnter " + other.name);
        SetInfo(Type);
    }

    private void OnTriggerExit(Collider other)
    {
        _otherCollider = null;
        //print("OnTriggerExit " + other.name);

        SetInfo(Type);
    }
    

    private void SetInfo(WormPartType type)
    {

        switch (type)
        {
            case WormPartType.Head:
                worm.HeadCollidedWith = _otherCollider;
                //worm.headContactNormal = normal;
               
                //Debug.DrawLine(snake.headPoint.PositionWorld, snake.midPoint.PositionWorld + normal, Color.red, 1f);
                break;

            case WormPartType.Mid:
               // worm.MidIsCollided = value;
             //   worm.midContactNormal = normal;
               
                //Debug.DrawLine(snake.midPoint.PositionWorld, snake.midPoint.PositionWorld + normal, Color.red, 1f);
                break;

            case WormPartType.Tail:
                worm.TailCollidedWith = _otherCollider;
               // worm.tailContactNormal = normal;              
                //Debug.DrawLine(snake.tailPoint.PositionWorld, snake.midPoint.PositionWorld + normal, Color.red, 1f);
                break;

            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (_otherCollider != null)
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawSphere(_otherCollider.ClosestPoint(this.transform.position), 0.1f);

            Gizmos.DrawSphere(_otherCollider.bounds.center, 0.1f);

            // Gizmos.DrawSphere(_otherCollider.bounds.center, 0.1f);
        }            
    }
}
