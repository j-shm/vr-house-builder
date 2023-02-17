using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSGWindow : Object
{
    private Collider col;
    [SerializeField]
    public Vector3 size;
    private float centerBound;
    private float fowardBound;

    private Wall wallScript;
    private GameObject currentWall;
    private Wall oldWallScript;
    [SerializeField]
    private GameObject MeshObject;
    [SerializeField]

    void Start()
    {
        Setup();
        col = GetComponent<Collider>();
        size = col.bounds.size;
        centerBound = col.bounds.center.y;
        fowardBound = size.z/2;
        
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
        spot = wallScript.CalculateClosestPoint(invis,this.gameObject);
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
            ChangeDrawings();
            return;
        }

        ChangeDrawings();

        //pos.z += fowardBound;
        gameObject.transform.position = spot;



        wallScript.AddWindow(this.MeshObject);
        wallScript.Cut();
        
        //you have to add the foward bound after the hole is cut or it won't properly cut
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,
        gameObject.transform.position.y,
        gameObject.transform.position.z+fowardBound);

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
                transform.position = new Vector3(initalPos.x,initalPos.y,initalPos.z-fowardBound);
                oldWallScript.AddWindow(this.MeshObject);
                oldWallScript.Cut();
                transform.position = new Vector3(initalPos.x,initalPos.y,initalPos.z+fowardBound);
            }
            }
        
        isHeld = !isHeld;
        if(isHeld && oldWallScript != null) {
            oldWallScript.RemoveWindow(this.MeshObject);
            oldWallScript.Cut();
        }
        initalPos = transform.position;
        ChangeDrawings(isHeld);
    }
}
