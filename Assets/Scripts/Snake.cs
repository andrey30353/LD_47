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

    Vector3 _input;

    // чем управляем сейчас
    bool useHead;

    // куда смотрим - через координаты
    bool rightView;

    float step => (float)1 / (_parts.Count - 1);
      
    public bool HeadIsCollided = false;
    public Collider HeadCollidedWith;
    public bool MidIsCollided = false;
    public bool TailIsCollided = false;
    public Collider TailCollidedWith;

    float CurrentDistance => (headPoint.PositionWorld - tailPoint.PositionWorld).magnitude;
    float CurrentDistanceX => headPoint.PositionWorld.x - tailPoint.PositionWorld.x;

    public SoundEffector soundEffector;

    public bool onGround;

    public Vector3 headContactNormal;
    public Vector3 midContactNormal;
    public Vector3 tailContactNormal;

    [SerializeField, Range(0f, 90f)]
    public float maxGroundAngle = 25f;
    public float minGroundDotProduct;

    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    void Awake()
    {       
        OnValidate();
    }

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
        prevLength = Length;

        //UpdateBones();
    }

    // Update is called once per frame
    void Update()
    {

        /*
        if (CurrentDistance < minDistance)
        {
            if (useHead)
            {
                headPoint.PositionWorld = headPoint.PositionWorld + Vector3.up * Speed * Time.deltaTime;
            }
            else
            {
                tailPoint.PositionWorld = tailPoint.PositionWorld + Vector3.up * Speed * Time.deltaTime;
            }
        }*/
               

        if (Input.GetKeyDown(KeyCode.R))
        {
            headPoint.ControlFirstLocal += Vector3.right * 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            headPoint.ControlFirstLocal += Vector3.left * 0.1f;
            //foreach (var item in _parts)
            //{
            //    item.Rotate(new Vector3(0, 90, 0));
            //}
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            useHead = !useHead;
        }

        if (CurrentDistance > Length)
        {
            //print("CurrentDistance > Length");
            if (useHead)
            {
                var direction = (tailPoint.PositionWorld - headPoint.PositionWorld).normalized;
                headPoint.PositionWorld = headPoint.PositionWorld + direction * Speed * Time.deltaTime;
            }
            else
            {
                var direction = (headPoint.PositionWorld - tailPoint.PositionWorld).normalized;
                tailPoint.PositionWorld = tailPoint.PositionWorld + direction * Speed * Time.deltaTime;
            }
        }

        _input = Vector3.zero;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        _input = new Vector3(horizontal, vertical, 0);

        //print(headPoint.PositionWorld);
        //print(tailPoint.PositionWorld);
        UpdatePosition();

        // отменяем все, если длина увеличилась       
        if (IsIncorrectSnake())
        {
            RevertPosition();
        }

        // выталкиваем вверх из препятствия
        if(_input != Vector3.zero)
        {
            if (useHead)
            {
                if (HeadIsCollided)
                {
                    var direction = headContactNormal;//Vector3.up;// (HeadCollidedWith.center - headPoint.PositionWorld).normalized;
                    headPoint.PositionWorld = headPoint.PositionWorld + direction * Speed * Time.deltaTime;
                }
            }
            else
            {
                if (TailIsCollided)
                {
                    var direction = tailContactNormal;// Vector3.up;// (HeadCollidedWith.center - headPoint.PositionWorld).normalized;
                    tailPoint.PositionWorld = tailPoint.PositionWorld + direction * Speed * Time.deltaTime;
                }
            }

            if (MidIsCollided)
            {
                if (useHead)
                    tailPoint.PositionWorld = tailPoint.PositionWorld + Vector3.up * Speed * 0.5f * Time.deltaTime;
                else
                    headPoint.PositionWorld = headPoint.PositionWorld + Vector3.up * Speed * 0.5f * Time.deltaTime;
            }

        }

        ApplyGravity();

        UpdateMidPoint();

        CorrectControlPoints();

        UpdateBones();

        // постепенное увеличение гравитации чтобы в начале игры не провалится
        gravity += Time.deltaTime;
        gravity = Mathf.Clamp(gravity, 0, MaxGravity);
    }
   
    private bool IsIncorrectSnake()
    {     
        var partOfLength = Length * 0.2f;
        var isPointPositionIncorrect = 
            midPoint.PositionWorld.x + partOfLength >= headPoint.PositionWorld.x  || 
            tailPoint.PositionWorld.x + partOfLength >= midPoint.PositionWorld.x;
        return CurrentDistance > Length /*|| CurrentDistance < minDistance*/ || isPointPositionIncorrect;
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

        midPoint.PositionWorld = middle + Vector3.up * freeLength * 0.8f;      
    }

    private void CorrectControlPoints()
    {
        if (CurrentDistance < Length)
        {
            headPoint.ControlFirstLocal = new Vector3(0.5f * CurrentDistanceX / Length  , 0 , 0);
            midPoint.ControlFirstLocal = new Vector3(0.5f * CurrentDistanceX / Length , 0, 0);
            tailPoint.ControlFirstLocal = new Vector3(0.5f * CurrentDistanceX / Length, 0, 0);
        }       
    }

    private void ApplyGravity()
    {
        if (MidIsCollided)
            return;

        if(!TailIsCollided && !HeadIsCollided)
        {
            headPoint.PositionWorld = headPoint.PositionWorld + Vector3.down * gravity *  Time.deltaTime;
            tailPoint.PositionWorld = tailPoint.PositionWorld + Vector3.down * gravity *  Time.deltaTime;
            return;
        }

        if (useHead)
        {
            if (TailIsCollided)
                return;

            tailPoint.PositionWorld = tailPoint.PositionWorld + Vector3.down * 1 * Time.deltaTime;
        }
        else
        {
            if (HeadIsCollided)
                return;

            headPoint.PositionWorld = headPoint.PositionWorld + Vector3.down * 1 * Time.deltaTime;
        }
        
        // отменяем все, если длина увеличилась       
        if (CurrentDistance > Length )
        {
            if (useHead)
            {
                tailPoint.PositionWorld = tailPoint.PositionWorld - Vector3.down * gravity * Time.deltaTime;
            }
            else
            {
                headPoint.PositionWorld = headPoint.PositionWorld - Vector3.down * gravity * Time.deltaTime;
            }
        }
    }

    public void Dead()
    {
        mesh.material.color = Color.grey;
        this.enabled = false;

        foreach (var item in _parts)
        {
            var collider = item.GetComponent<SphereCollider>();
            if(collider != null)
            {
                collider.isTrigger = true;
                item.gameObject.AddComponent<CantMove>();
            }

            var rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(rb);
            }
        }
    }


    private void OnDrawGizmos()
    {
        return;

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

    /*
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        print(other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Collide"))
        {
           // var player = other.GetComponentInParent<Snake>();
            //player.HeadIsCollided = true;
            print("Collide - OnTriggerEnter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Collide"))
        {           
            print("Collide - OnTriggerExit");
        }       
    }*/

}
