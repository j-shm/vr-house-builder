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
        if(currentWallCollider != null) {
            return this.currentWallCollider.ClosestPoint(point);

        } else {
            return this.wallCollider.ClosestPoint(point);
        }

    }


    public Vector3 GetNearestValidPoint(Window baseWindow, Vector3 point) {
        
        Vector3 nearpoint = GetNearestPoint(point);

        /*
        GameObject cube = CubeCreator(baseWindow);

        Collider col = cube.GetComponent<Collider>();
        */

        return nearpoint;
    }
}
