using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClosestPoint : MonoBehaviour
{
    public GameObject toSee;
    public bool closestPoint;
    private LineRenderer cpline;
    public bool goTo;
    public GameObject child;
    void Start() {
        cpline = this.gameObject.AddComponent<LineRenderer>();
        cpline.endColor = Color.cyan;
        cpline.startColor = Color.cyan;
        cpline.endWidth = 0.3f;
        cpline.startWidth = 0.3f;
    }
    void Update()
    {

        if(toSee == null) {
            return;
        }
        this.gameObject.transform.rotation = toSee.transform.rotation;
        Vector3 point;
        if(closestPoint) {
            //HAVE TO USE THIS
            point = toSee.GetComponent<Collider>().ClosestPoint(this.gameObject.transform.position);
        } else {
            point = toSee.GetComponent<Collider>().ClosestPointOnBounds(this.gameObject.transform.position);
        }



        cpline.SetPosition(0,transform.position);
        cpline.SetPosition(1,point);

        if(goTo && toSee != null) {
            moveToYGameObject(point);
            goTo = false;
        }
    }
    void moveToYGameObject(Vector3 point)
    {
        Vector3 AbsoluteMovement = point - 
        child.transform.position;
        this.transform.position += AbsoluteMovement;
    }
    }
