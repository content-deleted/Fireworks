using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamNode : MonoBehaviour
{
    void OnCollisionStay(Collision collision)  {
        if(collision.rigidbody.tag == "Player") {
            NodeBasedCamera.currentNode = this.transform;
        }
    }
}
