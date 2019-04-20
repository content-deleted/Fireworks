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
            rb.velocity = (currentNode.position - transform.position) / 10;
        }
    }
}
