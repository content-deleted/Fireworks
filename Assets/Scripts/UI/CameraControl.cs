using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float minDistance;
    public float maxDistance;
    public float maxY;
    private Rigidbody r;
    public float forceAmount;
    public float yForce;
    public float heightOffset;
    public void Awake () {
        r = GetComponent<Rigidbody>();
    }

    public void LateUpdate () {
        var playerP = new Vector2(PlayerState.singleton.transform.position.x, PlayerState.singleton.transform.position.z);
        var camP =  new Vector2(transform.position.x, transform.position.z);
        var dist = Vector3.Distance(camP, playerP);
        r.velocity /= 1.5f;

        // find Y val
        var pY = PlayerState.singleton.transform.position.y + heightOffset;
        var yDist = Mathf.Abs(pY - transform.position.y);
        float yV = 0;
        if( yDist > maxY) yV = (pY - transform.position.y) * yForce;

        if(dist > maxDistance) {
            var dir =(playerP - camP);
            r.velocity = new Vector3(dir.x, yV, dir.y) * forceAmount; 
        }
        else if(dist < minDistance) {
            var dir =(camP - playerP);
            r.velocity = new Vector3(dir.x, yV, dir.y) * forceAmount; 
        }
        else if(yV != 0) r.velocity = new Vector3(r.velocity.x, yV, r.velocity.z) * forceAmount; 
    }
}
