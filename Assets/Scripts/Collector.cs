using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public Transform spinningBone;
    // This is whats called when we run into a trigger collider 
    public void OnTriggerEnter(Collider other) {
        // tag compare to see if we can pick it up
        other.transform.parent = spinningBone;

        
    }
}
