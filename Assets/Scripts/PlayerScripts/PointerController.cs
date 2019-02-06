using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    const int layerNum = 1 << 9; 

    public GameObject grabPoint;
    private Rigidbody heldObject;
    private GameObject line;
    private LineRenderer lr;
    void Update()
    {
        if(  PlayerState.singleton.pointerMode && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ){ 
            //DrawLine(transform.position, transform.position + transform.forward*20, Color.yellow, 0.3f);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))//, layerNum))
            {
                //Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
                var r = hit.rigidbody;

                if (r!=null) {
                    heldObject = r;
                    DrawLine(transform.position, hit.point, Color.yellow, 0.3f);

                    grabPoint.transform.position = hit.point;
                    grabPoint.transform.parent = transform;
                }
            }
        }
        else{
            if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && heldObject != null){
                GameObject.Destroy(line);
                heldObject = null;
                grabPoint.transform.parent = null;
            }
        }
        if(heldObject != null) {
            heldObject.velocity += (grabPoint.transform.position - heldObject.transform.position)/2;
            lr.SetPosition(1, grabPoint.transform.position);
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
