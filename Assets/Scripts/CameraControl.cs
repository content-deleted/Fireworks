using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float minDistance;
    public float maxDistance;
    private Rigidbody r;

    public float maxSpeed;
    public float forceAmount;
    public void Awake () {
        r = GetComponent<Rigidbody>();
    }

    public void LateUpdate () {
        var dist = Vector3.Distance(transform.position, PlayerState.singleton.transform.position);
        r.velocity /= 2;
        if(dist > maxDistance)
            r.velocity = (PlayerState.singleton.transform.position - transform.position).normalized * forceAmount; //r.AddForce( (PlayerState.singleton.transform.position - transform.position).normalized * forceAmount, ForceMode.Force);
        if(dist < minDistance)
            r.velocity = (transform.position - PlayerState.singleton.transform.position).normalized * forceAmount; //r.AddForce((transform.position - PlayerState.singleton.transform.position).normalized * forceAmount, ForceMode.Force);
        Debug.Log(dist);
    }
}
