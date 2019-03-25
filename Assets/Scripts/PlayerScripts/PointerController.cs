using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointerController : MonoBehaviour
{
    const int layerNum = 1 << 9; 
    public static PointerController singleton;
    public GameObject grabPoint;
    public GameObject lightCursor;
    public GameObject pointEnd;
    public GameObject handModel;
    public OVRScreenFade screenfade;
    public bool creationMode = false;
    private Rigidbody heldObject;
    private GameObject line;
    private LineRenderer lr;
    private LineBendController lineBend;
    private Animator animator;
    public int segments = 20;
    public Material lineMat;
    private float prevRotation; 
    private bool couldRotate;
    private bool redOrBlue;
    void Awake() {
        line = new GameObject();
        line.AddComponent<LineRenderer>();
        lr = line.GetComponent<LineRenderer>();

        line.AddComponent<LineBendController>();
        lineBend = line.GetComponent<LineBendController>();
        lineBend.holdPoint = grabPoint;

        lr.material = lineMat;//new Material(Shader.Find("Custom/LineBend"));
        lr.startWidth = 0.05f;
        lr.endWidth =  0.1f;

        line.SetActive(false);
        pointEnd.SetActive(false);

        lr.positionCount = segments;

        // Store a reference to the hand's animator
        animator = handModel.GetComponent<Animator>();

        singleton = this;
    }
    const int ignoreMask = ~(1 << 12);
    void Update()
    {
        if(PlayerState.singleton.pointerMode && PauseManager.singleton?.Paused!=true) { 
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreMask))//, layerNum))
            {
                var r = hit.rigidbody;
                // if the player is holding down
                if( OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ) {
                    if (r!=null && r.gameObject.tag != "Player") {
                        heldObject = r;
                        heldObject.useGravity = false;
                        couldRotate = heldObject.freezeRotation; // store previous rotation setting
                        heldObject.freezeRotation = true;
                        
                        line.transform.position = pointEnd.transform.position;
                        updateSegments(pointEnd.transform.position, heldObject.transform.position);
                        line.SetActive(true);

                        grabPoint.transform.position = hit.point;
                        grabPoint.transform.parent = transform;
                        lightCursor.SetActive(false);
                        pointEnd.SetActive(true);

                        animator.SetBool("Pointing", true);
                        animator.SetTrigger("StartPoint");

                        prevRotation = handModel.transform.rotation.eulerAngles.z;
                    }
                    if(creationMode && r.gameObject.tag == "Player") {
                        // Make a popup
                        screenfade.FadeOut( () => SceneManager.LoadScene("FireworkScene") );
                    }
                }
                // update the point light
                else {
                    lightCursor.transform.position = hit.point;

                    // Update color of cursor
                    var rob = r != null && r.tag != "Player";
                    if(redOrBlue != rob) {
                        lightCursor.GetComponent<Renderer>().material.SetColor("_Color", (rob) ? new Color(0.85f,0.45f,0.15f,1) : new Color(0.6f,0.6f,0.95f,1));
                        lightCursor.GetComponent<Pulse>().enabled = rob;
                    }
                    redOrBlue = rob;
                }
            }
            // LET GO OF OBJECT
            if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && heldObject != null){
                disableLine ();
            }
            // HOLDING OBJECT update
            else if(heldObject != null) {
                // Check input for moving object depth
                Vector2 primaryTouchpad = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
                if(primaryTouchpad.y != 0 && primaryTouchpad.y>0 || Vector3.Distance(transform.position, grabPoint.transform.position) > 4.5f)
                    grabPoint.transform.position += 0.3f * transform.forward * primaryTouchpad.y;

                Vector3 curRotation = new Vector3(0,0,handModel.transform.rotation.eulerAngles.z - prevRotation);
                heldObject.transform.Rotate(handModel.transform.forward, curRotation.z);
                prevRotation = handModel.transform.rotation.eulerAngles.z;

                Vector3 toObject = grabPoint.transform.position - heldObject.transform.position;
                heldObject.velocity *= 0.85f;
                heldObject.velocity += toObject;

                updateSegments(pointEnd.transform.position, heldObject.transform.position);
            }
        }
        else {
            // LET GO IF WE SWITCH MODES
            if(heldObject != null) {
                disableLine ();
            }
        }
    }

    public void disableLine () {
        line.SetActive(false);

        heldObject.useGravity = true;
        heldObject.freezeRotation = couldRotate;

        heldObject = null;
        grabPoint.transform.parent = null;
        lightCursor.SetActive(true);
        pointEnd.SetActive(false);

        animator.SetBool("Pointing", false);
    }

    void updateSegments (Vector3 start, Vector3 end) {
        lr.positionCount = segments;
        for(int i = 0; i < segments; i++) {
            lr.SetPosition(i, Vector3.Lerp(start, end, (float)i / (float) segments));
        }
    }
}
