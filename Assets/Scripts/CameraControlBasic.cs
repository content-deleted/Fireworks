using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlBasic : MonoBehaviour
{
    public float minDistance;
    public float maxDistance;
    private Rigidbody r;
    public float forceAmount;
    public float heightOffset;
    public void Awake () {
        r = GetComponent<Rigidbody>();
    }

    public void LateUpdate () {
        var playerP = PlayerState.singleton.transform.position + Vector3.up * heightOffset;
        var dist = Vector3.Distance(transform.position, playerP);
        r.velocity /= 2;
        if(dist > maxDistance)
            r.velocity = (playerP - transform.position).normalized * forceAmount; 
        if(dist < minDistance)
            r.velocity = (transform.position - playerP).normalized * forceAmount;
    }
}
