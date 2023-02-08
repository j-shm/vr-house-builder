using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : Object
{
    private Collider col;
    [SerializeField]
    public Vector3 size;
    private float centerBound;
    private float fowardBound;

    private CustomWindowMesh wallScript;
    private GameObject currentWall;

    void Start()
    {
        Setup();
        col = GetComponent<Collider>();
        size = col.bounds.size;
        centerBound = col.bounds.center.y;
        fowardBound = size.z/2;
    }


    private void Update() {
        if(!isHeld) return;

        Collider[] walls = Physics.OverlapSphere(transform.position, 1f,(1<<7));
        GameObject closestObject = null;
        CustomWindowMesh script = null;
        float closestWallDist = -1;
        foreach(var wall in walls) {
            var currentTrans = wall.transform.position;
            var newDist = Vector3.Distance(transform.position,currentTrans);
            if(closestWallDist < newDist) {
                var _script = wall.gameObject.GetComponent<CustomWindowMesh>();
                if(_script.GetValidity(size)) {
                    closestObject = wall.gameObject;
                    closestWallDist = newDist;
                    script = _script;
                }
            }
        }
        if(closestObject != currentWall) {
            if(currentWall != null) {
                currentWall.GetComponent<CustomWindowMesh>().SetWindowSize(new Vector3(0,0,0));
                currentWall.GetComponent<CustomWindowMesh>().MakeWall();
            }
            currentWall = closestObject;

        }

        if(walls.Length == 0 || closestObject == null) {
            spot = new Vector3(-.01f,-.01f,-.01f);
            spotValid = false;
            ChangeDrawings(false);
            return;
        }




        wallScript = script;
        spotValid = true;

        float closestDist = -1;
        float ratio = 0.5f;
        var wallPoints = script.GetPoints();

        //this doesn't feel right in game
        for(int i =0; i< wallPoints.Count; i++) {
                var pos = wallPoints[i];
                var newDist = Vector3.Distance(transform.position,pos);
                if(closestDist < newDist) {
                    closestDist = newDist;
                    spot = pos;
                    ratio = (float)i * 0.25f;
                }
        }



        wallScript.SetWindowSize(size,ratio);
        wallScript.MakeWall();
        var tSpot = wallScript.GetCenter();
        tSpot.y -= centerBound;
        tSpot.z += fowardBound;
        spot = tSpot;
        Debug.Log(spot);
        if(spotValid) {
            DrawGuide();
            ChangeDrawings(true);
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
        wallScript.SetWindowSize(size,spot.z);
        wallScript.MakeWall();
        var pos = wallScript.GetCenter();
        pos.y -= centerBound;
        pos.z += fowardBound;
        gameObject.transform.position =  pos;
    }
    public override void DrawLine() {
        line.SetPosition(0,new Vector3(transform.position.x,transform.position.y + centerBound,transform.position.z));
        line.SetPosition(1,new Vector3(spot.x,spot.y + centerBound,spot.z));
    }
}
