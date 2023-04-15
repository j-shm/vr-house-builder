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

    private Wall wallScript;
    private GameObject currentWall;
    private Wall oldWallScript;
    private Window windowScript;

    private GameObject center;
    void Start()
    {
        Setup();
        windowScript = GetComponent<Window>(); //this doesnt seem right because i feel like there should be a method for this
        col = GetComponent<Collider>();
        invisCol = invis.GetComponent<Collider>();
        size = col.bounds.size;
        centerBound = col.bounds.center.y;
        center = transform.Find("center").gameObject;

        center.transform.localPosition = new Vector3(0,centerBound,-col.bounds.max.z);
    }


    private void Update() {

        if(!isHeld) return;

        Collider[] walls = Physics.OverlapSphere(transform.position, 3f,(1<<7));
        GameObject closestObject = null;
        Wall script = null;
        float closestWallDist = -1;
        
        //we need to get the closest wall.
        foreach(var wall in walls) {
            var currentTrans = wall.transform.position;
            var newDist = Vector3.Distance(transform.position,currentTrans);
            if(closestWallDist < newDist) {
                var _script = wall.gameObject.GetComponent<Wall>();
                closestObject = wall.gameObject;
                closestWallDist = newDist;
                script = _script;

                //we need to switch the rotation if it is behind it so the front side always matches.
                if(Vector3.Dot(closestObject.gameObject.GetComponent<Collider>().bounds.max-this.gameObject.transform.position,
                closestObject.gameObject.transform.forward) < 0) {
                    Vector3 rotVec = wall.gameObject.transform.rotation.eulerAngles;
                    rotVec.x *= -1; //flip x
                    rotVec.y = (rotVec.y + 180) % 360;
                    Quaternion rot = Quaternion.Euler(rotVec);
                    this.gameObject.transform.rotation = rot;
                } else {
                    this.gameObject.transform.rotation = wall.gameObject.transform.rotation;
                }
                

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

        spot = wallScript.GetNearestValidPoint(this.windowScript,this.gameObject.transform.position);
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
        
        if(deleteButton.action.WasPressedThisFrame()) {
            Kill();
        }

        ChangeDrawings();

        if(!spotValid) {
            //return to orginal position
            transform.position = initalPos;
            transform.rotation = initalRotation;
            return;
        }

        GoToObjectFromChild(); 
        wallScript.AddWindow(this.windowScript);
        wallScript.Cut();
        

        oldWallScript = wallScript;

    }

    //this is used to make sure the window lines up with the wall by moving the child to the position of the spot
    void GoToObjectFromChild() {
        Vector3 AbsoluteMovement = spot - 
        center.transform.position;
        AbsoluteMovement.y += centerBound;
        this.transform.position += AbsoluteMovement;
    }

    public override void DrawLine() {
        line.SetPosition(0,GetPos());
        line.SetPosition(1,new Vector3(spot.x,spot.y + GetOffset(),spot.z));
    }

    //get the position of the wall including the offset for the windows.
    public Vector3 GetPos() {
        return new Vector3(transform.position.x,transform.position.y + centerBound, transform.position.z);
    }
    public float GetOffset() {
        return centerBound;
    }

    //slight modification to the orginal method to make sure the window is placed on the wall 
    //as if the window was placed using place() it gets off pos by a little bit.
    public void LoadWindow() {
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
        script.AddWindow(this.windowScript);
        script.Cut();
        oldWallScript = script;

    }

    public override void SetHeld() {
        isHeld = !isHeld;

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


        if(isHeld && oldWallScript != null) {
            oldWallScript.RemoveWindow(this.windowScript);
            oldWallScript.Cut();
        }

        initalPos = transform.position;
        initalRotation = transform.rotation;
        ChangeDrawings(isHeld);
    }
}
