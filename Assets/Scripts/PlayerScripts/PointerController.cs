using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    const int layerNum = 1 << 9; 

    public GameObject grabPoint;
    public GameObject lightCursor;
    public GameObject pointEnd;
    private Rigidbody heldObject;
    private GameObject line;
    private LineRenderer lr;
    public int segments = 20;
    private LineBendController lineBend;
    void Awake() {
        line = new GameObject();
        line.AddComponent<LineRenderer>();
        lr = line.GetComponent<LineRenderer>();

        line.AddComponent<LineBendController>();
        lineBend = line.GetComponent<LineBendController>();
        lineBend.holdPoint = grabPoint;

        line.SetActive(false);

        lr.positionCount = segments;
    }
    void Update()
    {
        if(  PlayerState.singleton.pointerMode ){ 
            //DrawLine(transform.position, transform.position + transform.forward*20, Color.yellow, 0.3f);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))//, layerNum))
            {
                //Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
                var r = hit.rigidbody;
                // if the player is holding down
                if( OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ) {
                    if (r!=null) {
                        heldObject = r;
                        heldObject.useGravity = false;
                        DrawLine(pointEnd.transform.position, hit.point);

                        // update the effect
                        lineBend.holdPoint = heldObject.gameObject;

                        grabPoint.transform.position = hit.point;
                        grabPoint.transform.parent = transform;
                        lightCursor.SetActive(false);
                    }
                }
                // update the point light
                else {
                    lightCursor.transform.position = hit.point;
                }
            }
            // LET GO OF OBJECT
            if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && heldObject != null){
                disableLine ();
            }
            // HOLDING OBJECT update
            else if(heldObject != null) {
                Vector3 toObject = grabPoint.transform.position - heldObject.transform.position;
                heldObject.velocity *= 0.85f;
                heldObject.velocity += toObject;

                updateSegments(pointEnd.transform.position, grabPoint.transform.position);
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
        heldObject = null;
        grabPoint.transform.parent = null;
        lightCursor.SetActive(true);

        // update the effect
        lineBend.holdPoint = grabPoint;
    }
    void DrawLine(Vector3 start, Vector3 end)
    {
        line.SetActive(true);
        line.transform.position = start;
        lr.material = new Material(Shader.Find("Custom/LineBend"));
        lr.startWidth = 0.1f;
        lr.endWidth =  0.1f;

        updateSegments(start, end);
    }

    void updateSegments (Vector3 start, Vector3 end) {
        lr.positionCount = segments;
        for(int i = 0; i < segments; i++) {
            lr.SetPosition(i, Vector3.Lerp(start, end, (float)i / (float) segments));
        }
    }
}
