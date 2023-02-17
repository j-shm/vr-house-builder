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
    private Window currentWallScript;

    [SerializeField]
    private List<GameObject> windows = new List<GameObject>();
    private Material mat;
    public bool cut;
    private float buffer = 1f;

    private GameObject CutWall(GameObject wall, GameObject window) {
        Model result = CSG.Subtract(wall,window);
        GameObject newWall = new GameObject();
        newWall.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        newWall.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        return newWall;
    }

    private GameObject CutWalls() {
        GameObject returnWall = baseWall;
        GameObject destroyWall = null;
        foreach(GameObject window in windows) {
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
        wallCollider = baseWall.GetComponent<Collider>();
    }

    private void HandleCutting() {
        if(currentWall != null && currentWall != baseWall) {
            Destroy(currentWall);
        }
        currentWall = CutWalls();
        currentWall.transform.parent = this.gameObject.transform;
        if(currentWall != baseWall) {
            baseWallRend.enabled = false;
        } else {
            baseWallRend.enabled = true;
        }
    }
    
    //probably should be using an empty game object instead of the invis object.
    public Vector3 CalculateClosestPoint(GameObject window, GameObject heldWindow) {
        window.SetActive(true);
        Collider col = window.GetComponent<Collider>();
        Vector3 startPoint = wallCollider.bounds.ClosestPoint(heldWindow.transform.position);
        window.transform.position = startPoint;
        if(wallCollider.bounds.max.y < col.bounds.max.y) {
            window.transform.position -= new Vector3(0,(Mathf.Abs(wallCollider.bounds.max.y) - Mathf.Abs(col.bounds.max.y) - buffer),0);
        }
        if(wallCollider.bounds.min.y > col.bounds.min.y) {
            window.transform.position += new Vector3(0,(Mathf.Abs(wallCollider.bounds.max.y)+Mathf.Abs(col.bounds.max.y) + buffer),0);
        }

        if(wallCollider.bounds.max.x < col.bounds.max.x) {
            window.transform.position -= new Vector3((Mathf.Abs(wallCollider.bounds.max.x) - Mathf.Abs(col.bounds.max.x) - buffer),0,0);
        }
        if(wallCollider.bounds.min.x > col.bounds.min.x) {
            window.transform.position += new Vector3((Mathf.Abs(wallCollider.bounds.max.x)+Mathf.Abs(col.bounds.max.x) + buffer),0,0);
        }
        
        if(wallCollider.bounds.Contains(col.bounds.center)) {
            window.SetActive(false);
            return window.transform.position;
        } else {
            window.SetActive(false);
            return new Vector3(-.01f,-.01f,-.01f);
        }
    }
    

    public void AddWindow(GameObject window) {
        windows.Add(window);
    }
    public void RemoveWindow(GameObject window) {
        windows.Remove(window);
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

}
