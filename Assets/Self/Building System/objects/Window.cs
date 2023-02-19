using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : Object
{
    private Collider col;
    private Collider invisCol;
    [SerializeField]
    public Vector3 size;
    private float centerBound;
    public Vector2 fowardBound;
    private Wall wallScript;
    private GameObject currentWall;
    private Wall oldWallScript;
    [SerializeField]
    public GameObject MeshObject;
    private Window windowScript;

    void Start()
    {
        Setup();
        windowScript = GetComponent<Window>(); //this doesnt seem right but ii cant find method for doing it
        col = GetComponent<Collider>();
        invisCol = invis.GetComponent<Collider>();
        size = col.bounds.size;
        centerBound = col.bounds.center.y;

        fowardBound = new Vector2(size.z/2,size.x/2);
        
        if(col.GetType().Name.Equals("BoxCollider")) {
            Transform temp = transform.Find("Scene/cutout");
            if(temp == null) {
                MeshObject = transform.Find("Scene").gameObject.GetComponentsInChildren<Transform>()[1].gameObject;
            } else {
                MeshObject = temp.gameObject;
            }
        } else {
            MeshObject = this.gameObject;
        }
        
    }


    private void Update() {

        /*
        this is to stop the invis going really far away
        probably occurs because of the way the way the valid posistion is calculated
        in Wall.CalculateClosestPoint()  
        */
        if(Vector3.Distance(this.gameObject.transform.position,invis.transform.position) > 10) {
            invis.transform.position = this.gameObject.transform.position;
            spotValid = false;
        }
        if(!isHeld) return;

        Collider[] walls = Physics.OverlapSphere(transform.position, 3f,(1<<7));
        GameObject closestObject = null;
        Wall script = null;
        float closestWallDist = -1;
        
        foreach(var wall in walls) {
            var currentTrans = wall.transform.position;
            var newDist = Vector3.Distance(transform.position,currentTrans);
            if(closestWallDist < newDist) {
                var _script = wall.gameObject.GetComponent<Wall>();
                closestObject = wall.gameObject;
                closestWallDist = newDist;
                script = _script;
                this.gameObject.transform.rotation = wall.gameObject.transform.rotation;
            }
        }

        if(closestObject != currentWall) {
            currentWall = closestObject;
        }

        if(walls.Length == 0 || closestObject == null) {
            spot = new Vector3(-.01f,-.01f,-.01f);
            spotValid = false;
            ChangeDrawings(false);
            return;
        }

        wallScript = script;
        if(wallScript == null) {
            wallScript = closestObject.transform.parent.gameObject.GetComponent<Wall>();
        }
        spot = wallScript.CalculateClosestPoint(invis,this.gameObject,invisCol);
        if(spot.Equals(new Vector3(-.01f,-.01f,-.01f))) {
            spotValid = false;
        } else {
            spotValid = true;
        }
        
        
        if(spotValid) {
            DrawGuide();
            ChangeDrawings(true);
        } else {
            ChangeDrawings(false);
        }
    }

    private void Place() {
        if(!isHeld) return;
        if(!spotValid) {
            transform.position = initalPos;
            transform.rotation = initalRotation;
            ChangeDrawings();
            return;
        }

        ChangeDrawings();


        gameObject.transform.position = spot;
        wallScript.AddWindow(this.windowScript);
        wallScript.Cut();
        

        oldWallScript = wallScript;

    }
    public override void DrawLine() {
        line.SetPosition(0,new Vector3(transform.position.x,transform.position.y + centerBound,transform.position.z));
        line.SetPosition(1,new Vector3(spot.x,spot.y + centerBound,spot.z));
    }
    public override void SetHeld() {
        
        if(spotValid) {
            Place();
        } else {
            if(isHeld && oldWallScript != null) {
                this.gameObject.transform.position = initalPos;
                transform.rotation = initalRotation;
                oldWallScript.AddWindow(this.windowScript);
                oldWallScript.Cut();
            }
            }
        
        isHeld = !isHeld;
        if(isHeld && oldWallScript != null) {
            oldWallScript.RemoveWindow(this.windowScript);
            oldWallScript.Cut();
        }
        if(this.gameObject.transform.rotation.y == 0 || this.gameObject.transform.rotation.y == 1) {
            initalPos = new Vector3(transform.position.x,transform.position.y,transform.position.z-fowardBound[0]);
        } else {
            initalPos = new Vector3(transform.position.x-fowardBound[1],transform.position.y,transform.position.z);
        }
        initalRotation = transform.rotation;
        ChangeDrawings(isHeld);
    }
}
