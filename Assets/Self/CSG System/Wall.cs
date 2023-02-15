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
    private List<GameObject> windows = new List<GameObject>();
    private Material mat;
    public bool cut;

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
    }

    private void HandleCutting() {
        if(currentWall != null) {
            Destroy(currentWall);
        }
        currentWall = CutWalls();
        currentWall.transform.parent = this.gameObject.transform;
        baseWallRend.enabled = false;
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
