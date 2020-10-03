using UnityEngine;

public class CantMove : MonoBehaviour
{  
    public BoxCollider collider;
    Snake player;


    public Vector3 ColliderCenter => transform.TransformPoint(collider.center);
    public Vector3 ColliderSize => transform.TransformPoint(collider.bounds.size);

    void Awake()
    {
        collider = GetComponent<BoxCollider>();     
    }

    private void Update()
    {
        if (player == null)
            return;       
    }

    /*private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        //if (other.gameObject.tag == "Head")
        //{
        //    if(player != null)
        //        player.Speed = 0;
        //}
    }*/

    //void OnCollisionEnter(Collision collision)
    //{

    //}

    //void OnCollisionExit(Collision collision)
    //{

    //}

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        //print(other.name);
        if (other.gameObject.tag == "Head")
        {
            var player = other.GetComponentInParent<Snake>();
            player.HeadIsCollided = true;
            print("HeadCollide - OnTriggerEnter");
        }

        if (other.gameObject.tag == "Tail")
        {
            var player = other.gameObject.GetComponentInParent<Snake>();
            player.TailIsCollided = true;
            print("TailIsCollided - OnTriggerEnter");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        //print(other.name);
        if (other.gameObject.tag == "Head")
        {
            var player = other.GetComponentInParent<Snake>();
            player.HeadIsCollided = true;
            //print("HeadCollide - OnTriggerEnter");
        }

        if (other.gameObject.tag == "Tail")
        {
            var player = other.gameObject.GetComponentInParent<Snake>();
            player.TailIsCollided = true;
            //print("TailIsCollided - OnTriggerEnter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Head")
        {
            var player = other.gameObject.GetComponentInParent<Snake>();
            player.HeadIsCollided = false;
            print("HeadCollide - OnTriggerExit");
        }

        if (other.gameObject.tag == "Tail")
        {
            var player = other.gameObject.GetComponentInParent<Snake>();
            player.TailIsCollided = false;
            print("TailCollide - OnTriggerEnter");
        }
    }


}
