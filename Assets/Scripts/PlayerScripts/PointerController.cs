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
    private LineBendController lineBend;
    public int segments = 20;

    public Material lineMat;
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

        lr.positionCount = segments;
    }
    void Update()
    {
        if(PlayerState.singleton.pointerMode) { 
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))//, layerNum))
            {
                var r = hit.rigidbody;
                // if the player is holding down
                if( OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ) {
                    if (r!=null) {
                        heldObject = r;
                        heldObject.useGravity = false;
                        line.SetActive(true);
                        line.transform.position = pointEnd.transform.position;

                        updateSegments(pointEnd.transform.position, heldObject.transform.position);

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
        heldObject = null;
        grabPoint.transform.parent = null;
        lightCursor.SetActive(true);
    }

    void updateSegments (Vector3 start, Vector3 end) {
        lr.positionCount = segments;
        for(int i = 0; i < segments; i++) {
            lr.SetPosition(i, Vector3.Lerp(start, end, (float)i / (float) segments));
        }
    }
}
