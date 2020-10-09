using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public BGCurve Curve;
    public BGCcMath math;

    public float Speed = 10;
    public float Length = 6;

    public float MaxGravity = 10;
    private float gravity;

    public SphereCollider HeadCollider;
    public SphereCollider TailCollider;

    public GameObject ModelContent;
    public SkinnedMeshRenderer mesh;

    public List<Transform> _parts;

    public BGCurvePointI headPoint;
    public BGCurvePointI midPoint;
    public BGCurvePointI tailPoint;

    float minDistance;
    float prevLength;

    private Vector3 _inputHead;
    private Vector3 _inputTail;

    // куда смотрим - через координаты
    bool rightView;

    public bool HeadInWater;
    public bool TailInWater;

    public bool InWater => HeadInWater && TailInWater;

    float step => (float)1 / (_parts.Count - 1);

    public bool HeadIsCollided = false;
    public Collider HeadCollidedWith;
    public bool MidIsCollided = false;
    public bool TailIsCollided = false;
    public Collider TailCollidedWith;

    float CurrentDistance => (headPoint.PositionWorld - tailPoint.PositionWorld).magnitude;
    float CurrentDistanceX => Math.Abs(headPoint.PositionWorld.x - tailPoint.PositionWorld.x);
    float CurrentDistanceY => Math.Abs(headPoint.PositionWorld.y - tailPoint.PositionWorld.y);

    public SoundEffector soundEffector;

    [Header("test")]
    public float CurrentDistance_test;
    public float CurrentDistanceX_test;
    public float CurrentDistanceY_test;
    public float DistanceRelation ;

    /*
    public Vector3 headContactNormal;
    public Vector3 midContactNormal;
    public Vector3 tailContactNormal;

    [SerializeField, Range(0f, 90f)]
    public float maxGroundAngle = 25f;
    public float minGroundDotProduct;*/

    private void Test()
    {
        CurrentDistance_test = CurrentDistance;
        CurrentDistanceX_test = CurrentDistanceX;
        CurrentDistanceY_test = CurrentDistanceY;      
        DistanceRelation = (CurrentDistanceX + 1) / (CurrentDistanceY + 1);
    }

    /* void OnValidate()
     {
         minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
     }

     void Awake()
     {
         OnValidate();
     }*/

    void Start()
    {
        rightView = true;

        math = Curve.GetComponent<BGCcMath>();
        headPoint = Curve.Points[0];
        midPoint = Curve.Points[1];
        tailPoint = Curve.Points[2];

        Length = math.GetDistance();
        minDistance = Length * 0.5f;
        prevLength = Length;

        //UpdateBones();
    }

    float timeInWater = 0f;
    // Update is called once per frame
    void Update()
    {
        ProcessInput();
      
        UpdatePosition();

        Test();
        

        // отменяем все, если длина увеличилась       
        //if (IsIncorrectSnake())
        //{
        //    RevertPosition();
        //}

        // выталкиваем вверх из препятствия
        /*
        if (_inputHead != Vector3.zero)
        {
            if (HeadIsCollided)
            {
                var direction = headContactNormal;//Vector3.up;// (HeadCollidedWith.center - headPoint.PositionWorld).normalized;
                headPoint.PositionWorld = headPoint.PositionWorld + direction * Speed * Time.deltaTime;
            }

            if (MidIsCollided)
            {              
                    tailPoint.PositionWorld = tailPoint.PositionWorld + Vector3.up * Speed * 0.5f * Time.deltaTime;              
                    headPoint.PositionWorld = headPoint.PositionWorld + Vector3.up * Speed * 0.5f * Time.deltaTime;
            }
        }
        if (_inputTail != Vector3.zero)
        {
            if (TailIsCollided)
            {
                var direction = tailContactNormal;// Vector3.up;// (HeadCollidedWith.center - headPoint.PositionWorld).normalized;
                tailPoint.PositionWorld = tailPoint.PositionWorld + direction * Speed * Time.deltaTime;
            }
        }
        */
        //ApplyGravity();

        //CorrectLenght();

        UpdateMidPoint();

        //  CorrectControlPoints();

        UpdateBones();

        // постепенное увеличение гравитации чтобы в начале игры не провалится
        // gravity += Time.deltaTime;
        // gravity = Mathf.Clamp(gravity, 0, MaxGravity);
    }

    private void FixedUpdate()
    {
        //UpdatePosition();

        //if (InWater)
        //    timeInWater += Time.deltaTime;

        //if (timeInWater > 2f)
        //{
        //    //Dead();
        //    TimeBar.Instance.current = 0;

        //    return;
        //}

        if (isDead)
            DelayedDead();
    }

    private void CorrectLenght()
    {
        if (CurrentDistance > Length)
        {
            var direction1 = (midPoint.PositionWorld - headPoint.PositionWorld).normalized;
            headPoint.PositionWorld = headPoint.PositionWorld + direction1 * Speed * Time.deltaTime;

            var direction2 = (midPoint.PositionWorld - tailPoint.PositionWorld).normalized;
            tailPoint.PositionWorld = tailPoint.PositionWorld + direction2 * Speed * Time.deltaTime;
        }
    }

    private bool IsIncorrectSnake()
    {
        var partOfLength = Length * 0.2f;
        var isPointPositionIncorrect =
            midPoint.PositionWorld.x + partOfLength >= headPoint.PositionWorld.x ||
            tailPoint.PositionWorld.x + partOfLength >= midPoint.PositionWorld.x;
        return CurrentDistance > Length /*|| CurrentDistance < minDistance*/ || isPointPositionIncorrect;
    }

    float diag = 1.414214f;
    private void UpdatePosition()
    {
        if (_inputHead != Vector3.zero)
        {
            var nextPositionHead = HeadCollider.transform.position - HeadCollider.center + _inputHead * Speed * Time.deltaTime;

            var correctedInputHead = CorrectInputByCollision(nextPositionHead, _inputHead);
            correctedInputHead = CorrectInputByForm(tailPoint.PositionWorld, headPoint.PositionWorld, correctedInputHead);
            correctedInputHead = CorrectInputByFormHead(correctedInputHead);

            var nextHeadPosition = headPoint.PositionWorld + correctedInputHead * Speed * Time.deltaTime;
            headPoint.PositionWorld = nextHeadPosition; 
        }

        if (_inputTail != Vector3.zero)
        {
            var nextPositionTail = TailCollider.transform.position - TailCollider.center + _inputTail * Speed * Time.deltaTime;
            
            var correctedInputTail = CorrectInputByCollision(nextPositionTail, _inputTail);
            correctedInputTail = CorrectInputByForm(headPoint.PositionWorld, tailPoint.PositionWorld, correctedInputTail);
            correctedInputTail = CorrectInputByFormTail(correctedInputTail);

            var nextTailPosition = tailPoint.PositionWorld + correctedInputTail * Speed * Time.deltaTime;
            tailPoint.PositionWorld = nextTailPosition;                     
        }
    }

    private Vector3 CorrectInputByFormHead(Vector3 input)
    {
        var nextX = headPoint.PositionWorld.x + input.x * Speed * Time.deltaTime;
        if (nextX < tailPoint.PositionWorld.x)
            input.x = 0;

        return input;
    }

    private Vector3 CorrectInputByFormTail(Vector3 input)
    {
        var nextX = tailPoint.PositionWorld.x + input.x * Speed * Time.deltaTime;
        if (nextX > headPoint.PositionWorld.x)
            input.x = 0;

        return input;
    }

    private bool LengthIsCorrect(Vector3 positionWorld, Vector3 nextHeadPosition)
    {
        var length = (nextHeadPosition - positionWorld).magnitude;
        if (length <= Length)
            return true;

        return false;
    }   

    private Vector3 CorrectInputByForm(Vector3 otherPartPosition, Vector3 currentPosition, Vector3 input)
    {
        var nextPosition = currentPosition + input * Speed * Time.deltaTime;
        var correctLenght = LengthIsCorrect(otherPartPosition, nextPosition);      
        if (correctLenght)
        {

            return input;
        }
        else
        {           
            input = Vector3.zero;                      
        }
        return input;
    }

   

    private Vector3 CorrectInputByCollision(Vector3 nextPosition, Vector3 input)
    {
        var result = input;

        if (result == Vector3.zero)
            return result;

        //var np = cp + result * Speed * Time.deltaTime;
        ////HeadCollider
        //Collider[] colliderResult = new Collider[4];
        int layerMask = 1 << 8;

        var rayRight = new Ray(nextPosition, Vector3.right);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.right * HeadCollider.radius, Color.red);
        bool right = Physics.Raycast(rayRight, HeadCollider.radius, layerMask);
        if (right)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.right * HeadCollider.radius, Color.blue);
            if (result.x > 0)
                result.x = 0;
        }

        var rayLeft = new Ray(nextPosition, Vector3.left);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.left * HeadCollider.radius, Color.red);
        bool left = Physics.Raycast(rayLeft, HeadCollider.radius, layerMask);
        if (left)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.left * HeadCollider.radius, Color.blue);
            if (result.x < 0)
                result.x = 0;
        }

        var rayUp = new Ray(nextPosition, Vector3.up);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.up * HeadCollider.radius, Color.red);
        bool up = Physics.Raycast(rayUp, HeadCollider.radius, layerMask);
        if (up)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.up * HeadCollider.radius, Color.blue);
            if (result.y > 0)
                result.y = 0;
        }

        var rayDown = new Ray(nextPosition, Vector3.down);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.down * HeadCollider.radius, Color.red);
        bool down = Physics.Raycast(rayDown, HeadCollider.radius, layerMask);
        if (down)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.down * HeadCollider.radius, Color.blue);
            if (result.y < 0)
                result.y = 0;
        }

        var rayRighUp = new Ray(nextPosition, Vector3.up + Vector3.right);
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.right).normalized * HeadCollider.radius, Color.red);
        if (Physics.Raycast(rayRighUp, HeadCollider.radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.right).normalized * HeadCollider.radius, Color.blue);

            if (!right && result.x > 0 && result.y == 0)
            {
                result.y = input.x * -0.5f;
                result.x *= 0.5f;
            }
            else if (!up && result.y > 0 && result.x == 0)
            {
                result.x = result.y * -0.5f;
                result.y *= 0.5f;
            }
            else
            {
                if (result.y > 0)
                    result.y = 0;
                if (result.x > 0)
                    result.x = 0;
            }
        }

        var rayRightDown = new Ray(nextPosition, Vector3.down + Vector3.right);
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.right).normalized * HeadCollider.radius, Color.red);
        if (Physics.Raycast(rayRightDown, HeadCollider.radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.right).normalized * HeadCollider.radius, Color.blue);

            if (!right && result.x > 0 && result.y == 0)
            {
                result.y = result.x * 0.5f;
                result.x *= 0.5f;
            }
            else if (!down && result.y < 0 && result.x == 0)
            {
                result.x = result.y * 0.5f;
                result.y *= 0.5f;
            }
            else
            {
                if (result.x > 0)
                    result.x = 0;
                if (result.y < 0)
                    result.y = 0;
            }
        }

        var rayLeftUp = new Ray(nextPosition, Vector3.up + Vector3.left);
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.left).normalized * HeadCollider.radius, Color.red);
        if (Physics.Raycast(rayLeftUp, HeadCollider.radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.left).normalized * HeadCollider.radius, Color.blue);
            // нажато только налево и слева пусто
            if (!left && result.x < 0 && result.y == 0)
            {
                result.y = result.x * 0.5f;
                result.x *= 0.5f;
            }
            // нажато только вверх и вверху пусто
            else if (!up && result.y > 0 && result.x == 0)
            {
                result.x = result.y * 0.5f;
                result.y *= 0.5f;
            }
            else
            {
                if (result.x < 0)
                    result.x = 0;
                if (result.y > 0)
                    result.y = 0;
            }
        }

        var rayLeftDown = new Ray(nextPosition, Vector3.down + Vector3.left);
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.left).normalized * HeadCollider.radius, Color.red);
        if (Physics.Raycast(rayLeftDown, HeadCollider.radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.left).normalized * HeadCollider.radius, Color.blue);

            // нажато только налево и слева пусто
            if (!left && result.x < 0 && result.y == 0)
            {
                result.y = result.x * -0.5f;
                result.x *= 0.5f;
            }
            else if (!down && result.y < 0 && result.x == 0)
            {
                result.x = result.y * -0.5f;
                result.y *= 0.5f;
            }
            else
            {
                if (result.x < 0)
                    result.x = 0;
                if (result.y < 0)
                    result.y = 0;
            }
        }

        return result;
    }

    private void UpdateBones()
    {
        //get position at the center of the spline  
        Vector3 tangAtSplineCenter;

        _parts[0].position = math.CalcPositionAndTangentByDistanceRatio(0f, out tangAtSplineCenter);
        //Debug.Log(tangAtSplineCenter);
        _parts[0].rotation = Quaternion.LookRotation(tangAtSplineCenter) * Quaternion.Euler(-90, -90, 0);
        for (int i = 1; i < _parts.Count; i++)
        {
            // print(_parts[i]);
            var posAtSplineCenter = math.CalcPositionAndTangentByDistanceRatio(i * step, out tangAtSplineCenter);
            //Debug.Log(tangAtSplineCenter);
            _parts[i].position = posAtSplineCenter;
            _parts[i].rotation = Quaternion.LookRotation(tangAtSplineCenter) * Quaternion.Euler(-90, -90, 0);
        }
    }

    private void UpdateMidPoint()
    {
        var upVector = Vector3.up;
        if (!rightView)
            upVector = Vector3.down;

        var middle = (headPoint.PositionWorld + tailPoint.PositionWorld) * 0.5f;
        var freeLength = Mathf.Clamp(Length - CurrentDistance, 0, Length);

        midPoint.PositionWorld = middle /*+ Vector3.up * freeLength * 0.8f*/;
    }

    private void CorrectControlPoints()
    {
        if (CurrentDistance < Length)
        {
            headPoint.ControlFirstLocal = new Vector3(0.5f * CurrentDistanceX / Length, 0, 0);
            midPoint.ControlFirstLocal = new Vector3(0.5f * CurrentDistanceX / Length, 0, 0);
            tailPoint.ControlFirstLocal = new Vector3(0.5f * CurrentDistanceX / Length, 0, 0);
        }
    }

    private void ApplyGravity()
    {
        if (MidIsCollided)
            return;

        if (!TailIsCollided && !HeadIsCollided)
        {
            headPoint.PositionWorld = headPoint.PositionWorld + Vector3.down * gravity * Time.deltaTime;
            tailPoint.PositionWorld = tailPoint.PositionWorld + Vector3.down * gravity * Time.deltaTime;
            return;
        }

        if (!TailIsCollided)
        {
            tailPoint.PositionWorld = tailPoint.PositionWorld + Vector3.down * 1 * Time.deltaTime;
        }

        if (!HeadIsCollided)
        {
            headPoint.PositionWorld = headPoint.PositionWorld + Vector3.down * 1 * Time.deltaTime;
        }


        // отменяем все, если длина увеличилась       
        /*if (CurrentDistance > Length)
        {
            if (useHead)
            {
                tailPoint.PositionWorld = tailPoint.PositionWorld - Vector3.down * gravity * Time.deltaTime;
            }
            else
            {
                headPoint.PositionWorld = headPoint.PositionWorld - Vector3.down * gravity * Time.deltaTime;
            }
        }*/
    }

    private bool isDead = false;
    public void Dead()
    {
        isDead = true;
    }



    private void DelayedDead()
    {
        mesh.material.color = Color.grey;

        foreach (var item in _parts)
        {
            /*var collider = item.GetComponent<SphereCollider>();
            if(collider != null)
            {
                collider.isTrigger = true;
                item.gameObject.AddComponent<CantMove>();
            }   
            }*/

            item.gameObject.layer = 0;

            var rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                Destroy(rb);
            }
        }
        this.enabled = false;

    }

    float _inputHeadX, _inputHeadY, _inputTailX, _inputTailY;

    private void ProcessInput()
    {
        // голова
        _inputHead = Vector3.zero;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        _inputHead = new Vector3(horizontal, vertical, 0).normalized;

        //  хвост
        _inputTail = Vector3.zero;
        //print(_inputHead);
        if (_inputHead == Vector3.zero)
        {
            var horizontal1 = Input.GetAxis("Horizontal1");
            var vertical1 = Input.GetAxis("Vertical1");
            _inputTail = new Vector3(horizontal1, vertical1, 0).normalized;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var headColliderPosition = HeadCollider.transform.position - HeadCollider.center;
        /*Gizmos.DrawLine(headColliderPosition, headColliderPosition + Vector3.up * HeadCollider.radius);
        Gizmos.DrawLine(headColliderPosition, headColliderPosition + Vector3.down * HeadCollider.radius);
        Gizmos.DrawLine(headColliderPosition, headColliderPosition + Vector3.left * HeadCollider.radius);
        Gizmos.DrawLine(headColliderPosition, headColliderPosition + Vector3.right * HeadCollider.radius);

        Gizmos.DrawLine(headColliderPosition, headColliderPosition + (Vector3.up + Vector3.right).normalized * HeadCollider.radius);
        Gizmos.DrawLine(headColliderPosition, headColliderPosition + (Vector3.up + Vector3.left).normalized * HeadCollider.radius);
        Gizmos.DrawLine(headColliderPosition, headColliderPosition + (Vector3.down + Vector3.right).normalized * HeadCollider.radius);
        Gizmos.DrawLine(headColliderPosition, headColliderPosition + (Vector3.down + Vector3.left).normalized * HeadCollider.radius);*/

        //Gizmos.DrawSphere( HeadCollider.transform.position - HeadCollider.center, HeadCollider.radius);
        //  Gizmos.DrawSphere(headPoint.PositionWorld, HeadCollider.radius);
        /* if (HeadCollidedWith != null)
         {
             Gizmos.color = Color.red;
             Gizmos.DrawSphere(_otherCollider.ClosestPoint(this.transform.position), 0.1f);
         }
         */
        //return;
        /*
        var upVector = Vector2.up;
        if (!rightView)
            upVector = Vector2.down;

        var freeLength = Mathf.Clamp(Length - CurrentDistance, 0, Length);

        var middle = (headPoint.PositionWorld + tailPoint.PositionWorld) * 0.5f;// Vector3.Project(headPoint.PositionWorld, tailPoint.PositionWorld);
        var normal = Vector3.Cross(headPoint.PositionWorld, tailPoint.PositionWorld);

        var proj = GetProjected(headPoint.PositionWorld, tailPoint.PositionWorld, middle + Vector3.up * freeLength);

        var difX = headPoint.PositionWorld.x - tailPoint.PositionWorld.x;
        var difY = headPoint.PositionWorld.y - tailPoint.PositionWorld.y;
        var dd = Mathf.Sin(difX) + Mathf.Cos(difY);


        var cc = Vector3.Reflect(middle, Vector3.down);
        //Vector3 heading = target.position - transform.position;
        //Vector3 force = Vector3.Project(heading, railDirection);

        //print($"Length = {Length}; currentLength = {currentLength}");
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(middle, 0.1f);
        //Поворот точки на угол angle:
        // mp.x = point.x * cos(angle) — point.y* sin(angle); rotated_point.y = point.x * sin(angle) + point.y * cos(angle);

        Gizmos.color = Color.blue;
        //var mp = middle + Vector3.up * freeLength;
        //mp.x = middle.x * Mathf.Cos(angle) — middle.y* Mathf.Sin(angle);
        //mp.y = middle.x * sin(angle) + middle.y * cos(angle);
        Gizmos.DrawSphere(middle + Vector3.up * freeLength, 0.1f);
        //Gizmos.DrawSphere(middle + (Vector3.up * Mathf.Sin(difX) + Vector3.left *  Mathf.Cos(difY)) * freeLength, 0.1f);        
        */

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

    [ContextMenu("UpdateBonesLog")]
    public void UpdateBonesLog()
    {
        //get position at the center of the spline  
        Vector3 tangAtSplineCenter;

        var posAtSplineCenter = math.CalcPositionAndTangentByDistanceRatio(0f, out tangAtSplineCenter);
        //_parts[0].position = math.CalcPositionAndTangentByDistanceRatio(0f, out tangAtSplineCenter);
        Debug.Log("0 = " + tangAtSplineCenter);
        //_parts[0].rotation = Quaternion.LookRotation(tangAtSplineCenter) * Quaternion.Euler(0, 90, 0);
        for (int i = 1; i < _parts.Count; i++)
        {
            posAtSplineCenter = math.CalcPositionAndTangentByDistanceRatio(i * step, out tangAtSplineCenter);
            Debug.Log(i + " = " + tangAtSplineCenter);
            //_parts[i].position = posAtSplineCenter;
            //_parts[i].rotation = Quaternion.LookRotation(tangAtSplineCenter) * Quaternion.Euler(0, 90, 0);
        }
    }



}
