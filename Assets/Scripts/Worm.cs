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
    public SphereCollider MidCollider;
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

    float minDistanceX;

    public SoundEffector soundEffector;

    [Header("test")]
    public float CurrentDistance_test;
    public float CurrentDistanceX_test;
    public float CurrentDistanceY_test;
    public float DistanceRelation;
    public float rotationAngle;
    public float tanRotationAngle;
    public Vector3 rel2;

    public float rel;
    public Vector3 rot;

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
        DistanceRelation = (CurrentDistanceX) / (CurrentDistanceY);

        //1       
        Vector3 newDirection = (headPoint.PositionWorld - tailPoint.PositionWorld);
        //2
        float x = newDirection.x;
        float y = newDirection.y;
        rotationAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        //tanRotationAngle = Mathf.Tan(Mathf.Atan2(y, x)) * Mathf.Rad2Deg;
        tanRotationAngle = Mathf.Tan(Mathf.Atan2(y, x));

        if (rotationAngle <= 45)
            rel = tanRotationAngle;
        else
            rel = 1 / tanRotationAngle;

        rel2 = newDirection.normalized;
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
        math = Curve.GetComponent<BGCcMath>();
        headPoint = Curve.Points[0];
        midPoint = Curve.Points[1];
        tailPoint = Curve.Points[2];

        Length = math.GetDistance();
        minDistance = Length * 0.4f;
        prevLength = Length;

        minDistanceX = Length * 0.4f;

        //UpdateBones();
    }

    float timeInWater = 0f;
    // Update is called once per frame
    void Update()
    {
        ProcessInput();

        UpdatePosition();

        //Test();
             
        //ApplyGravity();

        //CorrectLenght();

        UpdateMidPoint();

        CorrectControlPoints();

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

    float diag = 1.414214f;
    private void UpdatePosition()
    {
        if (_inputHead != Vector3.zero)
        {
            var nextHeadColliderPosition = HeadCollider.transform.position - HeadCollider.center + _inputHead * Speed * Time.deltaTime;

            var correctedInputHead = CorrectInputByCollision(nextHeadColliderPosition, _inputHead, HeadCollider.radius);
            correctedInputHead = CorrectInputByForm(tailPoint.PositionWorld, headPoint.PositionWorld, correctedInputHead);
            correctedInputHead = CorrectInputByFormHead(correctedInputHead);

            var nextHeadPosition = headPoint.PositionWorld + correctedInputHead * Speed * Time.deltaTime;

            var midPointNextPosition = CalcMidPointPosition(nextHeadPosition, tailPoint.PositionWorld);
            bool midPointCollision = IsMidPointCollide(midPointNextPosition, MidCollider.radius);
            if (!midPointCollision)
            {
                headPoint.PositionWorld = nextHeadPosition;
            }           
        }

        if (_inputTail != Vector3.zero)
        {
            var nextTailColliderPosition = TailCollider.transform.position - TailCollider.center + _inputTail * Speed * Time.deltaTime;

            var correctedInputTail = CorrectInputByCollision(nextTailColliderPosition, _inputTail, TailCollider.radius);
            correctedInputTail = CorrectInputByForm(headPoint.PositionWorld, tailPoint.PositionWorld, correctedInputTail);
            correctedInputTail = CorrectInputByFormTail(correctedInputTail);

            var nextTailPosition = tailPoint.PositionWorld + correctedInputTail * Speed * Time.deltaTime;

            var midPointNextPosition = CalcMidPointPosition(headPoint.PositionWorld, nextTailPosition);
            bool midPointCollision = IsMidPointCollide(midPointNextPosition, MidCollider.radius);
            if (!midPointCollision)
            {
                tailPoint.PositionWorld = nextTailPosition;
            }          
        }
    }    

    private Vector3 CorrectInputByFormHead(Vector3 input)
    {
        var nextX = headPoint.PositionWorld.x + input.x * Speed * Time.deltaTime;
        if (nextX < tailPoint.PositionWorld.x + minDistanceX)
            input.x = 0;

        return input;
    }

    private Vector3 CorrectInputByFormTail(Vector3 input)
    {
        var nextX = tailPoint.PositionWorld.x + input.x * Speed * Time.deltaTime;
        if (nextX > headPoint.PositionWorld.x - minDistanceX)
            input.x = 0;

        return input;
    }

    private bool LengthIsCorrect(Vector3 otherPartPosition, Vector3 nextPosition, out bool? more)
    {
        var length = (nextPosition - otherPartPosition).magnitude;
        if (LengthMoreMax(length)) 
        {
            more = true;
            return false;
        }
        if (LengthLessMin(length))
        {
            more = false;
            return false;
        }
        more = null;
        return true;
    }

    private bool LengthMoreMax(float currentLengt)
    {
        return currentLengt > Length;
    }

    private bool LengthLessMin(float currentLengt)
    {
        return currentLengt < minDistance;
    }

    private Vector3 CorrectInputByForm(Vector3 otherPartPosition, Vector3 currentPosition, Vector3 input)
    {
        var nextPosition = currentPosition + input * Speed * Time.deltaTime;
        var correctLenght = LengthIsCorrect(otherPartPosition, nextPosition, out bool? more);
        if (correctLenght)
        {
            return input;
        }
        else
        {
            input = RotateInputForIncorrectLength(otherPartPosition, currentPosition, input, more);
        }
        return input;
    }

    private Vector3 RotateInputForIncorrectLength(Vector3 otherPartPosition, Vector3 currentPosition, Vector3 input, bool? more)
    {
        if (more.Value)
        {
            //print(input);
            // поворачиваем по кругу
            // --- направо
            if (input.x > 0 && IsInputTooWeak(input.y))
            {
                return RotateToPoint(otherPartPosition, currentPosition, Vector3.right, false);
            }
            // вверх
            if (input.y > 0 && IsInputTooWeak(input.x))
            {
                return RotateToPoint(otherPartPosition, currentPosition, Vector3.up, true);
            }
            // направо вверх
            if (input.x > 0.5f && input.y > 0.5f)
            {
                var direction = (Vector3.right + Vector3.up).normalized;
                return RotateToPoint(otherPartPosition, currentPosition, direction, false);
            }
            // вниз
            if (input.y < 0 && IsInputTooWeak(input.x))
            {
                return RotateToPoint(otherPartPosition, currentPosition, Vector3.down, true);               
            }
            // направо вниз
            if (input.x > 0.5f && input.y < -0.5f)
            {
                var direction = (Vector3.right + Vector3.down).normalized;
                return RotateToPoint(otherPartPosition, currentPosition, direction, false);             
            }

            // --- налево
            if (input.x < 0 && IsInputTooWeak(input.y))
            {
                return RotateToPoint(otherPartPosition, currentPosition, Vector3.left, false);
            }

            // налево вверх
            if (input.x < -0.5f && input.y > 0.5f)
            {
                var direction = (Vector3.left + Vector3.up).normalized;
                return RotateToPoint(otherPartPosition, currentPosition, direction, false);
            }

            // налево вниз
            if (input.x < -0.5f && input.y < -0.5f)
            {
                var direction = (Vector3.left + Vector3.down).normalized;
                return RotateToPoint(otherPartPosition, currentPosition, direction, false);
            }
        }

        return Vector3.zero;
    }   

    private bool IsInputTooWeak(float value)
    {
        return value > -0.5f && value < 0.5f;
    }

    private Vector3 RotateToPoint(Vector3 otherPartPosition, Vector3 currentPosition, Vector3 pointDirection, bool checkLength)
    {
        var targetPoint = otherPartPosition + pointDirection * Length;
        var direction = (targetPoint - currentPosition);
        if (direction.sqrMagnitude > 1)
        {
            direction.Normalize();
        }
        //Debug.DrawLine(currentPosition, currentPosition + direction, Color.yellow); 
        if (checkLength)
        {
            // перепроверяем длину
            if (LengthIsCorrect(otherPartPosition, currentPosition + direction * Speed * Time.deltaTime, out bool? more2))
                return direction;
            else
                return Vector3.zero;
        } 

        return direction;
    }

    private Vector3 CorrectInputByCollision(Vector3 nextPosition, Vector3 input, float radius)
    {
        var result = input;

        if (result == Vector3.zero)
            return result;

        //var np = cp + result * Speed * Time.deltaTime;
        ////HeadCollider
        //Collider[] colliderResult = new Collider[4];
        int layerMask = 1 << 8;

        var rayRight = new Ray(nextPosition, Vector3.right);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.right * radius, Color.red);
        bool right = Physics.Raycast(rayRight, radius, layerMask);
        if (right)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.right * radius, Color.blue);
            if (result.x > 0)
                result.x = 0;
        }

        var rayLeft = new Ray(nextPosition, Vector3.left);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.left * radius, Color.red);
        bool left = Physics.Raycast(rayLeft, radius, layerMask);
        if (left)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.left * radius, Color.blue);
            if (result.x < 0)
                result.x = 0;
        }

        var rayUp = new Ray(nextPosition, Vector3.up);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.up * radius, Color.red);
        bool up = Physics.Raycast(rayUp, radius, layerMask);
        if (up)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.up * radius, Color.blue);
            if (result.y > 0)
                result.y = 0;
        }

        var rayDown = new Ray(nextPosition, Vector3.down);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.down * radius, Color.red);
        bool down = Physics.Raycast(rayDown, radius, layerMask);
        if (down)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.down * radius, Color.blue);
            if (result.y < 0)
                result.y = 0;
        }

        var rayRighUp = new Ray(nextPosition, Vector3.up + Vector3.right);
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.right).normalized * radius, Color.red);
        if (Physics.Raycast(rayRighUp, radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.right).normalized * radius, Color.blue);

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
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.right).normalized * radius, Color.red);
        if (Physics.Raycast(rayRightDown, radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.right).normalized * radius, Color.blue);

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
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.left).normalized * radius, Color.red);
        if (Physics.Raycast(rayLeftUp, radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.left).normalized * radius, Color.blue);
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
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.left).normalized * radius, Color.red);
        if (Physics.Raycast(rayLeftDown, radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.left).normalized * radius, Color.blue);

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

    private bool IsMidPointCollide(Vector3 nextPosition, float radius)
    {        
        int layerMask = 1 << 8;

        var rayRight = new Ray(nextPosition, Vector3.right);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.right * radius, Color.red);
        bool right = Physics.Raycast(rayRight, radius, layerMask);
        if (right)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.right * radius, Color.blue);
            return true;
        }

        var rayLeft = new Ray(nextPosition, Vector3.left);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.left * radius, Color.red);
        bool left = Physics.Raycast(rayLeft, radius, layerMask);
        if (left)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.left * radius, Color.blue);
            return true;
        }

        var rayUp = new Ray(nextPosition, Vector3.up);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.up * radius, Color.red);
        bool up = Physics.Raycast(rayUp, radius, layerMask);
        if (up)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.up * radius, Color.blue);
            return true;
        }

        var rayDown = new Ray(nextPosition, Vector3.down);
        Debug.DrawLine(nextPosition, nextPosition + Vector3.down * radius, Color.red);
        bool down = Physics.Raycast(rayDown, radius, layerMask);
        if (down)
        {
            Debug.DrawLine(nextPosition, nextPosition + Vector3.down * radius, Color.blue);
            return true;
        }

        var rayRighUp = new Ray(nextPosition, Vector3.up + Vector3.right);
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.right).normalized * radius, Color.red);
        if (Physics.Raycast(rayRighUp, radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.right).normalized * radius, Color.blue);
            return true;
        }

        var rayRightDown = new Ray(nextPosition, Vector3.down + Vector3.right);
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.right).normalized * radius, Color.red);
        if (Physics.Raycast(rayRightDown, radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.right).normalized * radius, Color.blue);
            return true;
        }

        var rayLeftUp = new Ray(nextPosition, Vector3.up + Vector3.left);
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.left).normalized * radius, Color.red);
        if (Physics.Raycast(rayLeftUp, radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.up + Vector3.left).normalized * radius, Color.blue);
            return true;
        }

        var rayLeftDown = new Ray(nextPosition, Vector3.down + Vector3.left);
        Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.left).normalized * radius, Color.red);
        if (Physics.Raycast(rayLeftDown, radius, layerMask))
        {
            Debug.DrawLine(nextPosition, nextPosition + (Vector3.down + Vector3.left).normalized * radius, Color.blue);
            return true;
        }

        return false;
    }

    private void UpdateBones()
    {
        //get position at the center of the spline  
        Vector3 tangAtSplineCenter;

        _parts[0].position = math.CalcPositionAndTangentByDistanceRatio(0f, out tangAtSplineCenter);
        //Debug.Log(tangAtSplineCenter);
        var vector = Quaternion.Euler(0,270,0);
        _parts[0].rotation = Quaternion.LookRotation(tangAtSplineCenter, Vector3.forward) * vector;
        for (int i = 1; i < _parts.Count; i++)
        {
            // print(_parts[i]);
            var posAtSplineCenter = math.CalcPositionAndTangentByDistanceRatio(i * step, out tangAtSplineCenter);
            //Debug.Log(tangAtSplineCenter);
            _parts[i].position = posAtSplineCenter;
            _parts[i].rotation = Quaternion.LookRotation(tangAtSplineCenter, Vector3.forward) * vector;
        }
    }

    private void UpdateMidPoint()
    {
        var newPosition = CalcMidPointPosition(headPoint.PositionWorld, tailPoint.PositionWorld);
        midPoint.PositionWorld = newPosition;
    }

    private Vector3 CalcMidPointPosition(Vector3 head, Vector3 tail)
    {
        var middle = (head + tail) * 0.5f;
        var distance = (head - tail).magnitude;
        var freeLength = Mathf.Clamp(Length - distance, 0, Length);
        var direction = (tailPoint.PositionWorld - headPoint.PositionWorld);
        var normal = Vector3.Cross(direction, Vector3.forward).normalized;

        //Debug.DrawLine(middle, middle + normal, Color.yellow);
        var position =  middle + normal * freeLength * 0.8f;
        return position;
    }

    private void CorrectControlPoints()
    {
        if (CurrentDistance < Length)
        {
            var distanceValue = CurrentDistanceX / Length;

            //headPoint.ControlFirstLocal = new Vector3(0.5f * distanceValue, 0, 0);
            var direction = (headPoint.PositionWorld - tailPoint.PositionWorld).normalized;
            midPoint.ControlFirstLocal = new Vector3(0.5f * /*distanceValue **/ direction.x, 0.5f * /*distanceValue **/ direction.y);
            //tailPoint.ControlFirstLocal = new Vector3(0.5f * distanceValue, 0, 0);
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

    [ContextMenu("Debug")]
    public void Deb()
    {
        Start();

        Update();
    }

}
