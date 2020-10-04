using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SnakePartType
{
    Head,
    Mid, 
    Tail
}

public class SnakePart : MonoBehaviour
{    
    public Snake snake;

    public SnakePartType Type;

    void OnCollisionEnter(Collision collision)
    {
        //onGround = true;
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        //onGround = true;
        EvaluateCollision(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        //onGround = true;
        // EvaluateCollision(collision);
       SetInfo(Type, false, Vector3.zero);
    }

    private void EvaluateCollision(Collision collision)
    {
        var contactNorm = Vector3.up;
        var cantMove = collision.gameObject.GetComponent<CantMove>();
        if(cantMove != null)
        {
            if (cantMove.Type == ForceType.Down)
                contactNorm = Vector3.down;
          // print(cantMove.Type);
        }

        //print("EvaluateCollision = " + collision.collider.name);        
        /*for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            // Debug.Log($"{normal.y} ? {snake.minGroundDotProduct}");
           // if (normal.y >= snake.minGroundDotProduct)
            {
                //Debug.Log("normal.y >= snake.minGroundDotProduct");
                snake.onGround = true;
            //snake.contactNormal += normal;
            contactNorm += normal;
           

            //snake.headPoint.PositionWorld +=
            // Debug.Log("sdf");
           // Debug.DrawLine(collision.GetContact(i).point, collision.GetContact(i).point + normal, Color.red, 1f);
            }       
        }
        //if (contactNorm.y >= snake.minGroundDotProduct)
        //{
        //    contactNorm = Vector3.zero;
        //}
       */
        SetInfo(Type, true, contactNorm);
        //Bounce(Type, contactNorm);
    }

    private void SetInfo(SnakePartType type, bool value, Vector3 normal)
    {
        normal.z = 0;
        normal.Normalize();
        switch (type)
        {
            case SnakePartType.Head:
                snake.HeadIsCollided = value;
                snake.headContactNormal = normal; 
                //Debug.DrawLine(snake.headPoint.PositionWorld, snake.midPoint.PositionWorld + normal, Color.red, 1f);
                break;

            case SnakePartType.Mid:
                snake.MidIsCollided = value;
                snake.midContactNormal = normal;
                //Debug.DrawLine(snake.midPoint.PositionWorld, snake.midPoint.PositionWorld + normal, Color.red, 1f);
                break;

            case SnakePartType.Tail:
                snake.TailIsCollided = value;
                snake.tailContactNormal = normal;
                //Debug.DrawLine(snake.tailPoint.PositionWorld, snake.midPoint.PositionWorld + normal, Color.red, 1f);
                break;

            default:
                break;
        }
    }
    
    private void Bounce(SnakePartType type, Vector3 value)
    {
        value.z = 0;
        switch (type)
        {
            case SnakePartType.Head:
                snake.headPoint.PositionWorld += value * snake.Speed * Time.deltaTime ;
                break;

            case SnakePartType.Mid:
                snake.midPoint.PositionWorld += value * snake.Speed * Time.deltaTime ;
                break;

            case SnakePartType.Tail:
                snake.tailPoint.PositionWorld += value * snake.Speed * Time.deltaTime;
                break;

            default:
                break;
        }
    }
}
