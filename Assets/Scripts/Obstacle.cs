using UnityEngine;

public class Obstacle : MonoBehaviour
{  
    public Collider collider;
    Snake player;

    public ForceType Type = ForceType.Up;

    public bool Water = false;

    //public Vector3 ColliderCenter => transform.TransformPoint(collider.center);
    //public Vector3 ColliderSize => transform.TransformPoint(collider.bounds.size);

    void Awake()
    {
        collider = GetComponent<BoxCollider>();        
    }

    private void Update()
    {
        if (player == null)
            return;       
    }

    private void OnTriggerEnter(Collider other)
    {        
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        //print(other.name);
        if (other.gameObject.tag == "Head")
        {
            var player = other.GetComponentInParent<Worm>();
            player.HeadIsCollided = true;
            player.HeadCollidedWith = collider;
            //print("HeadCollide - OnTriggerEnter");
        }

        if (other.gameObject.tag == "Mid")
        {
            var player = other.gameObject.GetComponentInParent<Worm>();
            player.MidIsCollided = true;           
            //print("MidIsCollided - OnTriggerEnter");
        }

        if (other.gameObject.tag == "Tail")
        {
            var player = other.gameObject.GetComponentInParent<Worm>();
            player.TailIsCollided = true;
            player.TailCollidedWith = collider;
           // print("TailIsCollided - OnTriggerEnter");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        //print(other.name);
        if (other.gameObject.tag == "Head")
        {
            var player = other.GetComponentInParent<Worm>();
            player.HeadIsCollided = true;
            player.HeadCollidedWith = collider;           
        }

        if (other.gameObject.tag == "Mid")
        {
            var player = other.gameObject.GetComponentInParent<Worm>();
            player.MidIsCollided = true;
            //print("MidIsCollided - OnTriggerEnter");
        }

        if (other.gameObject.tag == "Tail")
        {
            var player = other.gameObject.GetComponentInParent<Snake>();
            player.TailIsCollided = true;
            player.TailCollidedWith = collider;          
        }
    }

    private void OnTriggerExit(Collider other)
    {       
        if (other.gameObject.tag == "Head")
        {
            var player = other.gameObject.GetComponentInParent<Snake>();
            player.HeadIsCollided = false;
            player.TailCollidedWith = null;
           // print("HeadCollide - OnTriggerExit");
        }

        if (other.gameObject.tag == "Mid")
        {
            var player = other.gameObject.GetComponentInParent<Snake>();
            player.MidIsCollided = false;
            //print("MidIsCollided - OnTriggerEnter");
        }

        if (other.gameObject.tag == "Tail")
        {
            var player = other.gameObject.GetComponentInParent<Snake>();
            player.TailIsCollided = false;
            player.TailCollidedWith = null;
            //print("TailCollide - OnTriggerEnter");
        }
    }


}
