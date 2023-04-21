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
            // note we cant use the currentWallCollider because it is a mesh collider that is not convexand 
            // it doesnt have a ClosestPoint function      
            this.wallCollider.enabled = true;
            var result = this.wallCollider.ClosestPoint(point);
            this.wallCollider.enabled = false;
            return result; 
        }
            
        return this.wallCollider.ClosestPoint(point);
    }

    
    public Vector3 GetNearestValidPoint(Window baseWindow, Vector3 point) {
        
        Vector3 nearpoint = GetNearestPoint(point);

        if(nearpoint == baseWindow.gameObject.transform.position) 
            return new Vector3(-.01f,-.01f,-.01f);
        
        float wallRotation = Mathf.Abs(this.gameObject.transform.rotation.eulerAngles.y);

        Collider windowCollider = baseWindow.gameObject.GetComponent<Collider>();

        if(wallRotation == 0 || wallRotation == 90 || wallRotation == 180 
        || wallRotation == 270|| wallRotation == 360) {
            if(wallCollider.bounds.max.y < windowCollider.bounds.max.y 
            || wallCollider.bounds.min.y > windowCollider.bounds.min.y) {
                return new Vector3(-.01f,-.01f,-.01f);
            }
            if(wallRotation == 90 || wallRotation == 270) {
                if(wallCollider.bounds.max.z < windowCollider.bounds.max.z 
                || wallCollider.bounds.min.z > windowCollider.bounds.min.z) {
                    return new Vector3(-.01f,-.01f,-.01f);
                }
            } else {
                if(wallCollider.bounds.max.x < windowCollider.bounds.max.x 
                || wallCollider.bounds.min.x > windowCollider.bounds.min.x) {
                    return new Vector3(-.01f,-.01f,-.01f);
                }
            }
        } else {
            Debug.Log("this wall does not have a valid rotation and therefore cannot be checked");
            //TODO: add a check for the other rotations
            //this could be done by firing a raycast from one side to the other
            //and checking if it hits the window twice before it hits the other side of the wall
            //if it does then the window is valid
        }

        //TODO: Change to OverlapAllocBox to improve performance
        var size = baseWindow.gameObject.GetComponent<Collider>().bounds.size;
        if(Physics.OverlapBox(nearpoint+new Vector3(0,baseWindow.GetOffset(),0),new Vector3(size.x /2,size.y/2,size.z/2), Quaternion.identity,1<<LayerMask.NameToLayer("Object")).Length > 0) {
            return new Vector3(-.01f,-.01f,-.01f);
        }

        return nearpoint;
    }
}
