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


    private GameObject CutWall(GameObject wall, Window windowScript) {
        GameObject window = windowScript.gameObject;
        float wallRot = wall.transform.rotation.y;
        GameObject cube = null;
        cube = CreateCube(windowScript);     
        
    
        Model result = CSG.Subtract(wall,cube);
        GameObject newWall = new GameObject();
        newWall.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        newWall.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        Destroy(cube);
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
    
    public void AddWindow(Window windowScript) {
        GameObject window = windowScript.gameObject;
        float winRot = window.gameObject.transform.rotation.y;       
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


    public Vector3 GetNearestPoint(Vector3 point) {
        if(currentWallCollider != wallCollider && currentWallCollider != null) {
            this.wallCollider.enabled = true;
            var result = this.wallCollider.ClosestPoint(point);
            this.wallCollider.enabled = false;
            return result; 
            //currentWallCollider.ClosestPoint(point) doesnt work as you cant use convext collider
        }
            
        return this.wallCollider.ClosestPoint(point);
    }

    
    public Vector3 GetNearestValidPoint(Window baseWindow, Vector3 point) {
        Debug.Log("i am " + this.gameObject.name);
        Vector3 nearpoint = GetNearestPoint(point);
        if(nearpoint == baseWindow.gameObject.transform.position)  {
            Debug.Log("nearpoint invalid.");
            return new Vector3(-.01f,-.01f,-.01f);
        }
            
        

        Debug.Log("max x of the wall: " + wallCollider.bounds.max.x);
        Debug.Log("max x of the window: " + baseWindow.gameObject.GetComponent<Collider>().bounds.max.x);
        Debug.Log(wallCollider.bounds.max.x < baseWindow.gameObject.GetComponent<Collider>().bounds.max.x);
        Debug.Log("min x of the wall: " + wallCollider.bounds.min.x);
        Debug.Log("min x of the window: " + baseWindow.gameObject.GetComponent<Collider>().bounds.min.x);
        Debug.Log(wallCollider.bounds.min.x > baseWindow.gameObject.GetComponent<Collider>().bounds.min.x);
        Debug.Log("max y of the wall: " + wallCollider.bounds.max.y);
        Debug.Log("max y of the window: " + baseWindow.gameObject.GetComponent<Collider>().bounds.max.y);
        Debug.Log(wallCollider.bounds.max.y < baseWindow.gameObject.GetComponent<Collider>().bounds.max.y);
        Debug.Log("min y of the wall: " + wallCollider.bounds.min.y);
        Debug.Log("min y of the window: " + baseWindow.gameObject.GetComponent<Collider>().bounds.min.y);
        Debug.Log(wallCollider.bounds.min.y > baseWindow.gameObject.GetComponent<Collider>().bounds.min.y);
        Debug.Log("max z of the wall: " + wallCollider.bounds.max.z);
        Debug.Log("max z of the window: " + baseWindow.gameObject.GetComponent<Collider>().bounds.max.z);
        Debug.Log(wallCollider.bounds.max.z < baseWindow.gameObject.GetComponent<Collider>().bounds.max.z);
        Debug.Log("min z of the wall: " + wallCollider.bounds.min.z);
        Debug.Log("min z of the window: " + baseWindow.gameObject.GetComponent<Collider>().bounds.min.z);
        Debug.Log(wallCollider.bounds.min.z > baseWindow.gameObject.GetComponent<Collider>().bounds.min.z);







        //TODO: Change to OverlapAllocBox to improve performance
        var size = baseWindow.gameObject.GetComponent<Collider>().bounds.size;
        if(Physics.OverlapBox(nearpoint+new Vector3(0,baseWindow.GetOffset(),0),new Vector3(size.x /2,size.y/2,size.z/2), Quaternion.identity,1<<LayerMask.NameToLayer("Object")).Length > 0) {
            Debug.Log("overlap");
            return new Vector3(-.01f,-.01f,-.01f);
        }

        //need to check if the window goes off the wall: just use bounds

        return nearpoint;
    }
}
