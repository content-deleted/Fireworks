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
                        DrawLine(pointEnd.transform.position, hit.point, Color.yellow, 0.3f);

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
                GameObject.Destroy(line);
                heldObject.useGravity = true;
                heldObject = null;
                grabPoint.transform.parent = null;
                lightCursor.SetActive(true);
            }
            // HOLDING OBJECT update
            else if(heldObject != null) {
                Vector3 toObject = grabPoint.transform.position - heldObject.transform.position;
                heldObject.velocity *= 0.85f;
                heldObject.velocity += toObject;
                lr.SetPosition(0, pointEnd.transform.position);
                lr.SetPosition(1, grabPoint.transform.position);
            }
        }
        else {
            // LET GO IF WE SWITCH MODES
            if(heldObject != null) {
                GameObject.Destroy(line);
                heldObject.useGravity = true;
                heldObject = null;
                grabPoint.transform.parent = null;
                lightCursor.SetActive(true);
            }
        }
    }
    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth =  0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        line = myLine;
    }
}
