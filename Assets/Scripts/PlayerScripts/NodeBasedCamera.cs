using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBasedCamera : MonoBehaviour
{
    public static Transform currentNode = null;
    Rigidbody rb;
    public void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void LateUpdate() {
        if(currentNode != null) {
            if(Vector3.Distance(currentNode.position, transform.position) > 0.05f)
                rb.velocity =  (currentNode.position - transform.position).normalized + (currentNode.position - transform.position) / 8;
            else
                rb.velocity = Vector3.zero;
        }

    }
}
