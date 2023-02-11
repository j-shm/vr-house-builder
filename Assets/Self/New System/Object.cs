using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


//TODO CREATING THE INVIS OBJECT
[RequireComponent(typeof(Rigidbody))]
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

    public ObjectDetails details;

    public string GetName() {
        return details.GetName();
    }
    public string GetDesc() {
        return details.GetDescription();
    }
    public void SetDetails(ObjectDetails details) {
        this.details = details;
    }

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

 
    void Start() {
        Setup();

    }
    void Reset() {
        Setup();
    }

    protected void Setup() {
        //SET UP INVIS
        Material green = (Material)Resources.Load("GREEN", typeof(Material));
        if(invis.TryGetComponent(out MeshRenderer mRend)) {
            mRend.material = green;
        }
        MeshRenderer[] MeshRenderers = invis.GetComponentsInChildren<MeshRenderer>(true);
        if(MeshRenderers.Length != 0) {
            foreach(MeshRenderer mRendr in MeshRenderers) {
                mRendr.material = green;
            }
        }
        invis.SetActive(false);

        //SETUP LINE RENDERER
        line.enabled = false;
        line.startWidth = 0.1059608f;
        line.endWidth = 0.1059608f;
        
        //RIGIDBODY SETUP
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;


        
        //COLLIDER SETUP
        if(gameObject.TryGetComponent(out MeshFilter meshF)) {
            MeshCollider mCol = gameObject.AddComponent<MeshCollider>();
            mCol.sharedMesh = meshF.mesh;
        } else {
            MeshFilter[] mshFilters = gameObject.GetComponentsInChildren<MeshFilter>(true);

            //check mesh collider perf at some point too might make more sense to do box colliders
            if(mshFilters.Length == 2) {
                MeshCollider mCol = gameObject.AddComponent<MeshCollider>();
                mCol.sharedMesh = mshFilters[0].mesh;
            }  else {
                Transform meshes = transform.Find("Scene");
                AddColliderAroundChildren(meshes.gameObject,gameObject);
            }
        }

        //CONTROLLER SETUP
        var xrG = gameObject.AddComponent<XRGrabInteractable>();
        xrG.throwOnDetach = false;
        xrG.trackRotation = false;
        xrG.selectEntered.AddListener(OnSelectEntered);
        xrG.selectExited.AddListener(OnSelectExited);


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

    private Collider AddColliderAroundChildren(GameObject assetModel, GameObject boxModel = null)
    {
        if(boxModel == null) {
            boxModel = assetModel;
        }

        var pos = assetModel.transform.localPosition;
        var rot = assetModel.transform.localRotation;
        var scale = assetModel.transform.localScale;

        // need to clear out transforms while encapsulating bounds
        assetModel.transform.localPosition = Vector3.zero;
        assetModel.transform.localRotation = Quaternion.identity;
        assetModel.transform.localScale = Vector3.one;

        // start with root object's bounds
        var bounds = new Bounds(Vector3.zero, Vector3.zero);
        if (assetModel.transform.TryGetComponent<Renderer>(out var mainRenderer))
        {
            bounds = mainRenderer.bounds;
        }

        var descendants = assetModel.GetComponentsInChildren<Transform>();
        foreach (Transform desc in descendants)
        {
            if(desc.gameObject.name == "EXCLUDE") {
                continue;
            }
            if (desc.TryGetComponent<Renderer>(out var childRenderer))
            {
                //if initialized to renderer bounds yet
                if (bounds.extents == Vector3.zero)
                    bounds = childRenderer.bounds;
                bounds.Encapsulate(childRenderer.bounds);
            }
        }

        var boxCol = boxModel.AddComponent<BoxCollider>();
        boxCol.center = bounds.center - assetModel.transform.position;
        boxCol.size = bounds.size;

        // restore transforms
        assetModel.transform.localPosition = pos;
        assetModel.transform.localRotation = rot;
        assetModel.transform.localScale = scale;
        return boxCol;
    }

}
