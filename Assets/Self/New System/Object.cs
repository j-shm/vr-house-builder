using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


//TODO CREATING THE INVIS OBJECT
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(LineRenderer))]
public class Object : MonoBehaviour
{
    [SerializeField]
    protected bool isHeld;
    protected LineRenderer line;
    private Grid grid;
    protected Vector3 spot;
    protected bool spotValid;

    [SerializeField]
    private GameObject invis;

    protected Vector3 initalPos;

    [SerializeField]
    public InputActionReference leftHand;

    void Awake() {
        grid = FindObjectOfType<Grid>();
        line = GetComponent<LineRenderer>();
    }
    
    public void SetHeld() {
        if(spotValid) {
            Place();
        } else {
            initalPos = transform.position;
        }
        isHeld = !isHeld;
        ChangeDrawings(isHeld);
    }
    public bool SetInvis(GameObject invis) {
        if(this.invis == null) {
            this.invis = invis;
            return true;
        }
        return false;
    }

 
    void Start()
    {
        Setup();

    }
    void Reset() {
        Setup();
    }
    protected void Setup() {

        line.enabled = false;

        
        var green = (Material)Resources.Load("GREEN", typeof(Material));
        if(invis.TryGetComponent(out MeshRenderer mRend)) {
            mRend.material = green;
        }
        var MeshRenderers = invis.GetComponentsInChildren<MeshRenderer>(true);
        if(MeshRenderers.Length != 0) {
            foreach(var mRendr in MeshRenderers) {
                mRendr.material = green;
            }
        }

        invis.SetActive(false);
        line.startWidth = 0.1059608f;
        line.endWidth = 0.1059608f;
        var rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        var xrG = gameObject.GetComponent<XRGrabInteractable>();
        xrG.throwOnDetach = false;
        xrG.trackRotation = false;

        


        xrG.selectEntered.AddListener(OnSelectEntered);
        xrG.selectExited.AddListener(OnSelectExited);
        
        var mCol = gameObject.GetComponent<MeshCollider>();
        if(gameObject.TryGetComponent(out MeshFilter meshF)) {
            mCol.sharedMesh = meshF.mesh;
        } else {
            var mshFilters = gameObject.GetComponentsInChildren<MeshFilter>(true);

            //check mesh collider perf at some point too might make more sense to do box colliders
            if(mshFilters.Length == 1) {
                mCol.sharedMesh = mshFilters[0].mesh;
            }  else {
                // we probably want to make a box collider if theres one more than one since
                // i dont't think a mesh collider with mutliple is accurate
                mCol.sharedMesh = mshFilters[0].mesh;
            }
        }
        gameObject.layer = 6;

    }

    protected virtual void OnSelectEntered(SelectEnterEventArgs args) => SetHeld();

    protected virtual void OnSelectExited(SelectExitEventArgs args) => SetHeld();



    void Update()
    {
        if(!isHeld) return;
        
        //get spot
        spot = grid.GetNearestValidPoint(transform.position);
        spotValid = grid.GetValidityPos(spot);
        
        //draw the gizmos
        if(spotValid) {
            DrawGuide();
        }

        //check for placing and rotating needs to be changed for vr port
        if(Input.GetMouseButtonDown(1)) {
            SetHeld();
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            Rotate();
        }

        float controllerValue = leftHand.action.ReadValue<Vector2>().x;
        if(Mathf.Abs(controllerValue) == 1) {
            Rotate(45*controllerValue);
        }
        


    }
    protected void DrawGuide() {
        DrawLine();
        DrawInvis();
    }
    public virtual void DrawLine() {
        line.SetPosition(0,transform.position);
        line.SetPosition(1,spot);
    }
    private void DrawInvis() {
        invis.transform.position = spot;
    }
    private bool Place() {
        if(!isHeld) return false;
        if(spotValid)  {
            transform.position = spot;
        } else {
            transform.position = initalPos;
        }
        return true;
    }
    private void Rotate(float angle = 45f) {
        transform.rotation = transform.rotation * Quaternion.Euler(0, 0, angle);
    }
    protected void ChangeDrawings() {
        invis.SetActive(!invis.activeSelf);
        line.enabled = !line.enabled;
    }
    protected void ChangeDrawings(bool value) {
        invis.SetActive(value);
        line.enabled = value;
    }
}
