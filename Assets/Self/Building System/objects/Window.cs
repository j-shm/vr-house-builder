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

    private GameObject center;
    void Start()
    {
        Setup();
        windowScript = GetComponent<Window>(); //this doesnt seem right but ii cant find method for doing it
        col = GetComponent<Collider>();
        invisCol = invis.GetComponent<Collider>();
        size = col.bounds.size;
        centerBound = col.bounds.center.y;
        center = transform.Find("center").gameObject;

        center.transform.localPosition = new Vector3(0,centerBound,-col.bounds.max.z);

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

                //we need to switch the rotation if it is behind it so the front side always matches.
                if(Vector3.Dot(closestObject.gameObject.GetComponent<Collider>().bounds.max-this.gameObject.transform.position,
                closestObject.gameObject.transform.forward) < 0) {
                    Vector3 rotVec = wall.gameObject.transform.rotation.eulerAngles;
                    rotVec.x *= -1; //flip x
                    rotVec.y = (rotVec.y + 180) % 360;
                    Debug.Log(rotVec);
                    Quaternion rot = Quaternion.Euler(rotVec);
                    this.gameObject.transform.rotation = rot;
                } else {
                    Debug.Log("else:");
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

        if(!spotValid) {
            transform.position = initalPos;
            transform.rotation = initalRotation;
            ChangeDrawings();
            return;
        }

        ChangeDrawings();


        GoToObjectFromChild();
        wallScript.AddWindow(this.windowScript);
        wallScript.Cut();
        

        oldWallScript = wallScript;

    }

    void GoToObjectFromChild() {
        Vector3 AbsoluteMovement = spot - 
        center.transform.position;
        AbsoluteMovement.y += centerBound;
        this.transform.position += AbsoluteMovement;
    }

    public override void DrawLine() {
        line.SetPosition(0,GetPos());
        line.SetPosition(1,new Vector3(spot.x,spot.y + centerBound,spot.z));
    }

    public Vector3 GetPos() {
        return new Vector3(transform.position.x,transform.position.y + centerBound, transform.position.z);
    }
    public float GetOffset() {
        return centerBound;
    }

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
        Debug.Log(script);
        script.AddWindow(this.windowScript);
        script.Cut();
        oldWallScript = script;

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

        initalPos = transform.position;
        initalRotation = transform.rotation;
        ChangeDrawings(isHeld);
    }
}
