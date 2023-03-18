using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private GameObject baseWall;
    private MeshRenderer baseWallRend;
    private Collider wallCollider;

    [SerializeField]
    private GameObject currentWall;
    [SerializeField]
    private Collider currentWallCollider;
    private Window currentWallScript;

    [SerializeField]
    private List<Window> windows = new List<Window>();
    private Material mat;
    public bool cut;
    private float buffer = 1f;


    private GameObject CutWall(GameObject wall, Window windowScript) {
        GameObject window = windowScript.gameObject;
        float wallRot = wall.transform.rotation.y;
        GameObject cube = null;
        if(window.transform.eulerAngles.y == 0 || window.transform.eulerAngles.y == 180) {
            cube = CreateCube(windowScript);
        } else {
            cube = CreateCube(windowScript);            
        }
        
    
        Model result = CSG.Subtract(wall,cube);
        GameObject newWall = new GameObject();
        newWall.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        newWall.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        //Destroy(cube);
        return newWall;
    }

    private GameObject CutWalls() {
        GameObject returnWall = baseWall;
        GameObject destroyWall = null;
        foreach(Window window in windows) {
            if(returnWall != baseWall) {
                destroyWall = returnWall;
            }
            returnWall = CutWall(returnWall,window);
            
            if(destroyWall != null) {
                Destroy(destroyWall);
            }
            
        }
        return returnWall;
    }
    void Start() {
        Setup();
    }
    private void Setup() {
        if(baseWall == null) {
            baseWall = this.gameObject;
        }
        baseWallRend = baseWall.GetComponent<MeshRenderer>();
        currentWallCollider = wallCollider = baseWall.GetComponent<Collider>();
    }

    private void HandleCutting() {
        if(currentWall != null && currentWall != baseWall) {
            Destroy(currentWall);
        }
        currentWall = CutWalls();
        currentWall.transform.parent = this.gameObject.transform;
        if(currentWall != baseWall) {
            
            
            baseWallRend.enabled = false;
            wallCollider.enabled = false;

            MeshCollider newCol = currentWall.AddComponent<MeshCollider>();
            newCol.sharedMesh = currentWall.GetComponent<MeshFilter>().mesh;
            currentWallCollider = newCol;
            currentWall.layer = 7;
            
        } else {
            currentWallCollider = wallCollider;
            baseWallRend.enabled = true;
            wallCollider.enabled = true;
        }
    }
    
    //probably should be using an empty game object instead of the invis object.
    public Vector3 CalculateClosestPoint(GameObject window, GameObject heldWindow, Collider col) {

        window.SetActive(true);
        Vector3 startPoint = currentWallCollider.bounds.ClosestPoint(heldWindow.transform.position);
        window.transform.position = startPoint;
        float windowRot =  window.transform.rotation.y;

        //caculate y bounds
        if(currentWallCollider.bounds.max.y < col.bounds.max.y) {
            window.transform.position -= new Vector3(0,(Mathf.Abs(currentWallCollider.bounds.max.y) - Mathf.Abs(col.bounds.max.y) - buffer),0);
        }
        if(currentWallCollider.bounds.min.y > col.bounds.min.y) {
            window.transform.position += new Vector3(0,(Mathf.Abs(currentWallCollider.bounds.max.y)+Mathf.Abs(col.bounds.max.y) + buffer),0);
        
        }  
        //if its a - wall
        if(windowRot == 0 || windowRot == 1) {
            //calculate x
            if(currentWallCollider.bounds.max.x < col.bounds.max.x) {
                window.transform.position -= new Vector3((Mathf.Abs(currentWallCollider.bounds.max.x) - Mathf.Abs(col.bounds.max.x) - buffer),0,0);
            }
            if(currentWallCollider.bounds.min.x > col.bounds.min.x) {
                window.transform.position += new Vector3((Mathf.Abs(currentWallCollider.bounds.max.x)+Mathf.Abs(col.bounds.max.x) + buffer),0,0);
            }
        } else {
            //calculate z
            Debug.Log("this is yes");
            if(currentWallCollider.bounds.max.z < col.bounds.max.z) {
                window.transform.position -= new Vector3(0,0,(Mathf.Abs(currentWallCollider.bounds.max.z) - Mathf.Abs(col.bounds.max.z) - buffer));
            }
            if(currentWallCollider.bounds.min.z > col.bounds.min.z) {
                window.transform.position += new Vector3(0,0,(Mathf.Abs(currentWallCollider.bounds.max.z)+Mathf.Abs(col.bounds.max.z) + buffer));
            }
            if(window.transform.eulerAngles.y == 270f) {
                window.transform.position += new Vector3(0.05f,0,0);
            } else if(window.transform.eulerAngles.y == 90f) {
                window.transform.position += new Vector3(-0.05f,0,0);
            }
        }

        if(currentWallCollider.bounds.Contains(col.bounds.center) ) {
            window.SetActive(false);
            return window.transform.position;
        } else {
            window.SetActive(false);
            return new Vector3(-.01f,-.01f,-.01f);
        }       
    }
    

    public void AddWindow(Window windowScript) {
        GameObject window = windowScript.gameObject;
        float winRot = window.gameObject.transform.rotation.y;
        if(winRot == 0 || winRot  == 1 ) {
            window.transform.position = new Vector3(window.transform.position.x,window.transform.position.y,window.transform.position.z+windowScript.fowardBound[0]);
        } else {
            window.transform.position = new Vector3(window.transform.position.x+windowScript.fowardBound[1]+0.05f,window.transform.position.y,window.transform.position.z);
        }
       
        windows.Add(windowScript);
    }
    public void RemoveWindow(Window windowScript) {
        windows.Remove(windowScript);
    }
    public void Cut() {
        HandleCutting();
    }

    private void OnValidate() {
        if(cut) {
            Cut();
            cut = false;
        }
    }

    private GameObject CreateCube(Window baseWindow) {
        return CubeCreator(baseWindow);
    }
    private GameObject CubeCreator(Window baseWindow, bool visual = false) {
        var extension = 10;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = baseWindow.gameObject.GetComponent<Collider>().bounds.size;
        cube.transform.position = baseWindow.gameObject.GetComponent<Window>().GetPos();
        cube.transform.rotation = baseWall.gameObject.transform.rotation;
        cube.transform.localScale = new Vector3(baseWindow.gameObject.GetComponent<Window>().size.x,baseWindow.gameObject.GetComponent<Window>().size.y,extension);

        if(!visual) {
            cube.GetComponent<MeshRenderer>().enabled = false;
        }
        return cube;
    }

}
