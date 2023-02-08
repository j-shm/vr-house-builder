using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

 
    void Start()
    {
        Setup();
    }
    protected void Setup() {
        line.enabled = false;
        invis.SetActive(false);
        line.startWidth = 0.1059608f;
        line.endWidth = 0.1059608f;
    }
    // Update is called once per frame
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
